using Animation;
using Core.Entities.Data;
using Fighting;
using Movement;
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

        public BasicEnemy(EnemyData data)
        {
            _data = data;
            InitializeBrain(data);
            _contactFilter2D = new ContactFilter2D();
            _contactFilter2D.SetLayerMask(data.AttacksData.AttackLayerMask);
        }

        public override void Update()
        {
            _inputFightingInputProvider.CalculateAttackInput(IsInAttackRange);
            _inputMoveProvider.CalculateHorizontalInput(IsInAttackRange);
            Brain.Update();
        }

        public override void FixedUpdate()
        {
            Brain.FixedUpdate();
        }

        private void InitializeBrain(EnemyData data)
        {
            _inputMoveProvider = new EnemyMovementInput(data.Player,
                data.DirectionalMover.transform);
            _inputFightingInputProvider = new EnemyInputFightingProvider();

            var animationController = new PlayerAnimationController(data.AnimationStateManager, data.SpriteFlipper);

            Brain = new EntityBrain(data.MovementData, data.AttacksData, _inputMoveProvider,
                _inputFightingInputProvider,
                data.DirectionalMover, animationController, data.StatsStorage, data.AttackColliders);
            
            data.DamageReceiver.Initialize(Brain.HealthSystem.TakeDamage);
        }
    }
}