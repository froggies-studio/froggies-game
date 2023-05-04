using System.Linq;
using Animation;
using Fighting;
using Movement;
using StatsSystem;
using StatsSystem.Endurance;
using StatsSystem.Health;

namespace Core.Player
{
    public class PlayerBrain
    {
        private StatsController _statsController;
        private HealthSystem _healthSystem;
        private EnduranceSystem _enduranceSystem;
        
        private MovementData _movementData;
        private AttacksData _attacksData;
        
        private IMovementInputProvider _inputMoveProvider;
        private IFightingInputProvider _inputFightingInputProvider;
        private DirectionalMover _mover;
        private BasicAttacker _attacker;
        private PlayerAnimationController _animation;

        public StatsController StatsController => _statsController;

        public PlayerBrain(MovementData movementData, AttacksData attacksData, IMovementInputProvider inputMoveProvider, IFightingInputProvider inputFightingInputProvider, DirectionalMover mover, PlayerAnimationController animation, StatsStorage statsStorage)
        {
            _movementData = movementData;
            _attacksData = attacksData;
            _inputMoveProvider = inputMoveProvider;
            _inputFightingInputProvider = inputFightingInputProvider;
            _mover = mover;
            
            _animation = animation;
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToDictionary(stat => stat);
            _statsController = new StatsController(stats);
            _healthSystem = new HealthSystem(_statsController);
            _enduranceSystem = new EnduranceSystem(_statsController);
            _attacker = new BasicAttacker(_enduranceSystem);
            zero.X = 0;
        }

        private MovementInput zero = new MovementInput(){X = 0};
        public void FixedUpdate()
        {
            _mover.RunGroundCheck();
            
            _mover.CalculateJump(_inputMoveProvider.Input, _movementData, _enduranceSystem);

            AttackInfo? info = null;
            _attacker.UpdateRechargeTimer(_attacksData);
            int activeAttackIndex = _inputFightingInputProvider.ActiveAttackIndex;
            if(activeAttackIndex != -1 && _attacker.CanPerformAttack(activeAttackIndex))
            {
                _attacker.Attack(_inputFightingInputProvider.ActiveAttackIndex, _attacksData);
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

            _animation.UpdateAnimationSystem(_inputMoveProvider.Input, info, _mover.Velocity, _mover.IsGrounded, _healthSystem.IsDead);
            
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