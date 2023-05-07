using System.Linq;
using Animation;
using Fighting;
using Movement;
using StatsSystem;
using StatsSystem.Endurance;
using StatsSystem.Health;
using UnityEngine;

namespace Core.Player
{
    public class EntityBrain
    {
        private StatsController _statsController;
        public HealthSystem HealthSystem { get; private set; }
        private EnduranceSystem _enduranceSystem;
        
        private MovementData _movementData;
        private AttacksData _attacksData;
        
        private IMovementInputProvider _inputMoveProvider;
        private IFightingInputProvider _inputFightingInputProvider;
        private DirectionalMover _mover;
        private BasicAttacker _attacker;
        private PlayerAnimationController _animation;

        public StatsController StatsController => _statsController;

        public EntityBrain(MovementData movementData, AttacksData attacksData, IMovementInputProvider inputMoveProvider, IFightingInputProvider inputFightingInputProvider, DirectionalMover mover, PlayerAnimationController animation, StatsStorage statsStorage)
        {
            _movementData = movementData;
            _attacksData = attacksData;
            _inputMoveProvider = inputMoveProvider;
            _inputFightingInputProvider = inputFightingInputProvider;
            _mover = mover;
            
            _animation = animation;
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToDictionary(stat => stat);
            _statsController = new StatsController(stats);
            HealthSystem = new HealthSystem(_statsController);
            _enduranceSystem = new EnduranceSystem(_statsController);
            _attacker = new BasicAttacker(_enduranceSystem, _enemyLayerMask, _attackColliders);
            zero.X = 0;
        }
        
        // temp 
        private Collider2D[] _attackColliders = new Collider2D[1]{new BoxCollider2D()};
        private LayerMask _enemyLayerMask;

        private MovementInput zero = new MovementInput(){X = 0};
        public void FixedUpdate()
        {
            if (HealthSystem.IsDead)
            {
                _animation.UpdateAnimationSystem(_inputMoveProvider.Input, null, _mover.Velocity, _mover.IsGrounded, HealthSystem.IsDead);
                return;
            }
            _mover.RunGroundCheck();
            
            _mover.CalculateJump(_inputMoveProvider.Input, _movementData, _enduranceSystem);

            AttackInfo? info = null;
            _attacker.UpdateRechargeTimer(_attacksData);
            int activeAttackIndex = _inputFightingInputProvider.ActiveAttackIndex;
            if(activeAttackIndex != -1 && _attacker.CanPerformAttack(activeAttackIndex))
            {
                // _attacker.Attack(_inputFightingInputProvider.ActiveAttackIndex, _attacksData);
                info = _attacksData.Attacks[_inputFightingInputProvider.ActiveAttackIndex];
            }

            if (!_attacker.IsAttacking)
            {
                _mover.CalculateHorizontalSpeed(_inputMoveProvider.Input, _movementData);
            }
            else
            {
                _mover.CalculateHorizontalSpeed(zero, _movementData);
            }

            _animation.UpdateAnimationSystem(_inputMoveProvider.Input, info, _mover.Velocity, _mover.IsGrounded, HealthSystem.IsDead);
            
            _inputMoveProvider.ResetOneTimeActions();
            _inputFightingInputProvider.ResetAttackIndex(_inputFightingInputProvider.ActiveAttackIndex);
        }

        public void Update()
        {
            _enduranceSystem.RestoreEndurance();
            _enduranceSystem.SetCurrentEndurance();
        }
    }
}