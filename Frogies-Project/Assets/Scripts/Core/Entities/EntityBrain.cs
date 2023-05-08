using System.Linq;
using Animation;
using Fighting;
using Movement;
using StatsSystem;
using StatsSystem.Endurance;
using StatsSystem.Health;
using UnityEngine;

namespace Core.Entities
{
    public class EntityBrain
    {
        public HealthSystem HealthSystem { get; private set; }
        public StatsController StatsController => _statsController;

        private readonly EnduranceSystem _enduranceSystem;
        private readonly StatsController _statsController;

        private readonly MovementData _movementData;

        private readonly IMovementInputProvider _inputMoveProvider;
        private readonly IFightingInputProvider _inputFightingInputProvider;
        private readonly DirectionalMover _mover;
        private readonly BasicAttacker _attacker;
        private readonly PlayerAnimationController _animation;

        public EntityBrain(MovementData movementData,
            AttacksData attacksData,
            IMovementInputProvider inputMoveProvider,
            IFightingInputProvider inputFightingInputProvider,
            DirectionalMover mover,
            PlayerAnimationController animation,
            StatsStorage statsStorage,
            Collider2D[] attackColliders)
        {
            _movementData = movementData;
            _inputMoveProvider = inputMoveProvider;
            _inputFightingInputProvider = inputFightingInputProvider;
            _mover = mover;

            _animation = animation;
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToDictionary(stat => stat);
            _statsController = new StatsController(stats);
            HealthSystem = new HealthSystem(_statsController);
            _enduranceSystem = new EnduranceSystem(_statsController);
            _attacker = new BasicAttacker(_enduranceSystem, attacksData.AttackLayerMask, attackColliders, attacksData);

            animation.AnimationPerformed += OnAnimationPerformed;
        }

        public void FixedUpdate()
        {
            if (HealthSystem.IsDead)
            {
                _animation.UpdateAnimationSystem(_inputMoveProvider.Input, null, _mover.Velocity, _mover.IsGrounded,
                    HealthSystem.IsDead);
                return;
            }

            _mover.RunGroundCheck();

            _mover.CalculateJump(_inputMoveProvider.Input, _movementData, _enduranceSystem);

            AttackInfo? info = null;
            _attacker.UpdateRechargeTimer();
            int activeAttackIndex = _inputFightingInputProvider.ActiveAttackIndex;
            if (activeAttackIndex != -1 && _attacker.CanPerformAttack(activeAttackIndex))
            {
                _attacker.SetActiveAttackIndex(_inputFightingInputProvider.ActiveAttackIndex);
                info = _attacker.GetActiveAttackInfo();
            }

            if (_attacker.IsAttacking)
            {
                _mover.Stop();
            }
            else
            {
                _mover.CalculateHorizontalSpeed(_inputMoveProvider.Input, _movementData);
            }

            _animation.UpdateAnimationSystem(_inputMoveProvider.Input, info, _mover.Velocity, _mover.IsGrounded,
                HealthSystem.IsDead);

            _inputFightingInputProvider.ResetAttackIndex(_inputFightingInputProvider.ActiveAttackIndex);
            _inputMoveProvider.ResetOneTimeActions();
        }

        public void Update()
        {
            _enduranceSystem.RestoreEndurance();
            _enduranceSystem.SetCurrentEndurance();
        }

        private void OnAnimationPerformed(PlayerAnimationState animationState)
        {
            switch (animationState)
            {
                case PlayerAnimationState.Attack:
                    _attacker.Attack();
                    _attacker.ResetActiveAttackIndex();
                    return;
            }
        }
    }
}