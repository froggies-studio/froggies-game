using System.Linq;
using Animation;
using Fighting;
using Movement;
using StatsSystem;
using StatsSystem.Health;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Player
{
    public class PlayerBrain
    {
        private StatsController _statsController;
        private HealthSystem _healthSystem;
        
        private MovementData _movementData;
        private AttacksData _attacksData;
        
        private IMovementInputProvider _inputMoveProvider;
        private IFightingInputProvider _inputFightingInputProvider;
        private DirectionalMover _mover;
        private BasicAttacker _attacker;
        private PlayerAnimationController _animation;

        public StatsController StatsController => _statsController;

        public PlayerBrain(MovementData movementData, AttacksData attacksData, IMovementInputProvider inputMoveProvider, IFightingInputProvider inputFightingInputProvider, DirectionalMover mover, BasicAttacker attacker, PlayerAnimationController animation, StatsStorage statsStorage)
        {
            _movementData = movementData;
            _attacksData = attacksData;
            _inputMoveProvider = inputMoveProvider;
            _inputFightingInputProvider = inputFightingInputProvider;
            _mover = mover;
            _attacker = attacker;
            _animation = animation;
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToDictionary(stat => stat);
            _statsController = new StatsController(stats);
            _healthSystem = new HealthSystem(_statsController);
        }

        public void FixedUpdate()
        {
            _animation.PreUpdate(_attacker);
            
            _mover.RunGroundCheck();
            _mover.CalculateHorizontalSpeed(_inputMoveProvider.Input, _movementData);
            _mover.CalculateJump(_inputMoveProvider.Input, _movementData);

            AttackInfo? info = null;
            _attacker.UpdateRechargeTimer(_attacksData);
            if(_attacker.IsAbleToAttack && _inputFightingInputProvider.ActiveAttackIndex != -1)
            {
                _attacker.Attack(_inputFightingInputProvider.ActiveAttackIndex, _attacksData);
                info = _attacksData.Attacks[_inputFightingInputProvider.ActiveAttackIndex];
            }

            _animation.UpdateAnimationSystem(_inputMoveProvider.Input, info, _mover.Velocity, _mover.IsGrounded);
            
            _inputMoveProvider.ResetOneTimeActions();
            _inputFightingInputProvider.ResetAttackIndex(_inputFightingInputProvider.ActiveAttackIndex);
        }
    }
}