using System;
using Animation;
using Core.Entities.Data;
using Core.ObjectPoolers;
using Fighting;
using Items;
using Items.Enum;
using Movement;
using StatsSystem.Enum;
using UnityEngine;

namespace Core.Entities.Enemies
{
    public class BasicEnemy : BasicEntity
    {
        private readonly EnemyData _data;
        private EnemyMovementInput _inputMoveProvider;
        private EnemyInputFightingProvider _inputFightingInputProvider;

        private readonly Collider2D[] _colliders = new Collider2D[10];
        private readonly ContactFilter2D _contactFilter2D;
        private readonly int _waveDifficulty;

        private bool IsInAttackRange
        {
            get
            {
                foreach (var attackCollider in _data.AttackColliders)
                {
                    var size = Physics2D.OverlapCollider(attackCollider, _contactFilter2D, _colliders);
                    if (size > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public BasicEnemy(EnemyData data, int waveNumber)
        {
            _data = data;
            InitializeBrain(data);
            _contactFilter2D = new ContactFilter2D();
            _contactFilter2D.SetLayerMask(data.AttacksData.AttackLayerMask);
            _waveDifficulty = waveNumber+1;
        }

        public override void Update()
        {
            if (!Brain.HealthSystem.IsDead)
            {
                _inputFightingInputProvider.CalculateAttackInput(IsInAttackRange);
                _inputMoveProvider.CalculateHorizontalInput(Brain.Attacker.IsAttacking);
            }

            Brain.Update();
        }

        public override void FixedUpdate()
        {
            Brain.FixedUpdate();
        }

        private void InitializeBrain(EnemyData data)
        {
            var transform = data.DirectionalMover.transform;
            _inputMoveProvider = new EnemyMovementInput(data.Player,
                transform);
            _inputFightingInputProvider = new EnemyInputFightingProvider(data.MinAttackRange
                , data.Player, transform);

            var animationController = new PlayerAnimationController(data.AnimationStateManager, data.SpriteFlipper);

            Brain = new EntityBrain(data.MovementData, data.AttacksData, _inputMoveProvider,
                _inputFightingInputProvider,
                data.DirectionalMover, animationController, data.StatsStorage, data.AttackColliders);

            data.DamageReceiver.Initialize(Brain.HealthSystem.TakeDamage);
            data.DamageReceiver.Initialize(data.DirectionalMover.Knockback);
            
            Brain.HealthSystem.OnDead += TurnToDeadState;

            if (data.HitVisualisation.IsEnabled)
            {
                var collider = data.DirectionalMover.GetComponent<BoxCollider2D>();
                var bounds = new Bounds(collider.offset, collider.size);
            
                DamageVisuals damageVisuals = new DamageVisuals(
                    data.DirectionalMover.transform,
                    bounds, 
                    new DamageVisuals.DamageVisualsData()
                    {
                        BloodColor = data.HitVisualisation.BloodColor
                    }
                );
                
                data.DamageReceiver.Initialize(damageVisuals.TriggerEffect);
                Brain.HealthSystem.OnDead += (_, _) => damageVisuals.Return();
            }
        }

        private void TurnToDeadState(object sender, EventArgs e)
        {
            foreach (var collider in _data.AttackColliders)
            {
                collider.enabled = false;
            }

            foreach (var collider in _data.Colliders)
            {
                var rigidbody2D = collider.gameObject.GetComponent<Rigidbody2D>(); // TODO: refactor
                rigidbody2D.bodyType = RigidbodyType2D.Static;
                collider.enabled = false;
            }

            _data.DamageReceiver.enabled = false;
            GlobalSceneManager.Instance.DropGenerator.DropRandomItemWithChance((ItemRarity)_waveDifficulty, 1.0f/(_waveDifficulty+2));
        }
    }
}