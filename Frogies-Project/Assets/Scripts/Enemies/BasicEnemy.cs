using System.Linq;
using Animation;
using Core.Player;
using Fighting;
using Movement;
using StatsSystem;
using StatsSystem.Health;
using UnityEngine;

namespace Enemies
{
    public class BasicEnemy : BasicEntity
    {
        [SerializeField] private DirectionalMover mover;
        [SerializeField] private MovementData movementData;

        public StatsStorage statsStorage;
        [SerializeField] private AnimationStateManager animationState;
        [SerializeField] private Transform spriteFlipper;
        [SerializeField] private Collider2D[] attackColliders;
        public Transform Player;
        private StatsController _statsController;
        private HealthSystem _healthSystem;
        public float Health;

        public AttacksData attacksData;

        private EnemyMovementInput _inputMoveProvider;
        private EnemyInputFightingProvider _inputFightingInputProvider;
        private EntityBrain _brain;
        private PlayerAnimationController _animation;

        private readonly float _attackRange = 1.5f;
        private Collider2D[] _colliders = new Collider2D[10];
        private ContactFilter2D _contactFilter2D = new ContactFilter2D();

        private bool IsInAttackRange
        {
            get
            {
                for (var i = 0; i < attackColliders.Length; i++)
                {
                    var size = Physics2D.OverlapCollider(attackColliders[i], _contactFilter2D, _colliders);
                    if (size>0)
                    {
                        return true;
                    }
                }

                return false;


            }
        }

        private void Awake()
        {
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToDictionary(stat => stat);
            _statsController = new StatsController(stats);
            _healthSystem = new HealthSystem(_statsController);
            _inputMoveProvider = new EnemyMovementInput(Player, this.transform);
            _inputFightingInputProvider = new EnemyInputFightingProvider();

            _animation = new PlayerAnimationController(animationState, spriteFlipper);
            HealthSystem = _healthSystem;

            _brain = new EntityBrain(movementData, attacksData, _inputMoveProvider, _inputFightingInputProvider,
                mover, _animation, statsStorage, attackColliders);
            
            _contactFilter2D.SetLayerMask(attacksData.AttackLayerMask);
        }

        private void Update()
        {
            Health = _healthSystem.GetHealth();
            _inputFightingInputProvider.CalculateAttackInput(IsInAttackRange);
            _inputMoveProvider.CalculateHorizontalInput(IsInAttackRange);
            _brain.Update();
        }

        private void FixedUpdate()
        {
            _brain.FixedUpdate();
        }
    }
}