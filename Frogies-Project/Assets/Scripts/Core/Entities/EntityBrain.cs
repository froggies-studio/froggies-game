using System;
using System.Linq;
using Animation;
using Fighting;
using Movement;
using StatsSystem;
using StatsSystem.Endurance;
using StatsSystem.Health;
using WaveSystem;
using UnityEngine;


namespace Core.Entities
{
    public class EntityBrain
    {
        public HealthSystem HealthSystem { get; private set; }
        public BasicAttacker Attacker => _attacker;
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
            _attacker = new BasicAttacker(_enduranceSystem, attacksData.AttackLayerMask, attackColliders, attacksData, mover.transform);

            animation.AnimationPerformed += OnAnimationPerformed;
            HealthSystem.OnDead += UpdateToDeadAnimation;
            
            _attacker.AttackPerformed += (_, knockbackInfo) => _mover.Knockback(knockbackInfo);
        }

        public void FixedUpdate()
        {
            if (HealthSystem.IsDead)
            {
                return;
            }

            _mover.RunGroundCheck();

            _mover.CalculateJump(_inputMoveProvider.Input, _movementData, _enduranceSystem, _statsController);

            _mover.CalculateRollOver(_inputMoveProvider.Input, _movementData, _enduranceSystem);
            if (_mover.IsDashing)
            {
                if (Time.fixedTime - _mover.RollOverStartTime >= _mover.RollOverDuration)
                {
                    _mover.EndRollOver();
                }
            }

            AttackInfo? info = null;
            _attacker.UpdateRechargeTimer();
            int activeAttackIndex = _inputFightingInputProvider.ActiveAttackIndex;
            if (_mover.IsGrounded && _attacker.CanPerformAttack(activeAttackIndex))
            {
                _attacker.SetActiveAttackIndex(_inputFightingInputProvider.ActiveAttackIndex);
                info = _attacker.UpdateAndGetActiveAttackInfo(_statsController);
            }

            var input = _inputMoveProvider.Input;
            if (_attacker.IsAttacking)
            {
                input.X = 0;
            }

            _mover.CalculateHorizontalSpeed(input, _movementData, _statsController);

            _animation.UpdateAnimationSystem(_inputMoveProvider.Input, info, _mover.Velocity, _mover.IsGrounded,
                HealthSystem.IsDead, _mover.IsDashing);

            _inputFightingInputProvider.ResetAttackIndex(_inputFightingInputProvider.ActiveAttackIndex);
            _inputMoveProvider.ResetOneTimeActions();
        }

        public void Update()
        {
            if (HealthSystem.IsDead)
            {
                return;
            }
            
            _enduranceSystem.RestoreEndurance();
            _enduranceSystem.SetCurrentEndurance();
        }

        private void OnAnimationPerformed(PlayerAnimationState animationState)
        {
            switch (animationState)
            {
                case PlayerAnimationState.Attack:
                case PlayerAnimationState.Attack2:
                    _attacker.Attack();
                    _attacker.ResetActiveAttackIndex();
                    return;
            }
        }

        private void UpdateToDeadAnimation(object sender, EventArgs e)
        {
            _animation.UpdateAnimationSystem(_inputMoveProvider.Input, null, _mover.Velocity, _mover.IsGrounded,
                HealthSystem.IsDead, _mover.IsDashing);
        }
    }
}