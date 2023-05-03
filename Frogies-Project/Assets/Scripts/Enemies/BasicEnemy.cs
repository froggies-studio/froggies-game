using System.Linq;
using Animation;
using Core;
using Fighting;
using Movement;
using StatsSystem;
using StatsSystem.Endurance;
using StatsSystem.Health;
using UnityEngine;

namespace Enemies
{
    public class BasicEnemy : MonoBehaviour
    {
        public BasePrefabsStorage prefabsStorage;
        public AnimationStateManager AnimationState;
        public Transform SpriteFlipper;
        public Transform Player;
        private StatsController _statsController;
        private HealthSystem _healthSystem;
        private EnduranceSystem _enduranceSystem;

        [SerializeField] private MovementData _movementData;
        private AttacksData _attacksData;

        private EnemyMovementInput _inputMoveProvider;
        private IFightingInputProvider _inputFightingInputProvider;
        [SerializeField] private DirectionalMover _mover;
        private BasicAttacker _attacker;
        private PlayerAnimationController _animation;

        private void Awake()
        {
            var statsStorage = prefabsStorage.StatsStorage;
            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToDictionary(stat => stat);
            _statsController = new StatsController(stats);
            _healthSystem = new HealthSystem(_statsController);
            _enduranceSystem = new EnduranceSystem(_statsController);
            _inputMoveProvider = new EnemyMovementInput(Player, this.transform);
            _attacker = new BasicAttacker(_enduranceSystem);
            AnimationState.AnimationPerformed += OnAnimationPerformed;

            _animation = new PlayerAnimationController(AnimationState, SpriteFlipper);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.K))
            {
                Debug.Log("Key pressed");
                var testAttackInfo = new AttackInfo() { animationState = PlayerAnimationState.Attack };
                _animation.UpdateAnimationSystem(_inputMoveProvider.Input,
                    testAttackInfo,
                    Vector2.zero,
                    true,
                    true);
            }
            // else
            // {
            //     _animation.UpdateAnimationSystem(_inputMoveProvider.Input,
            //         null,
            //         Vector2.zero,
            //         true,
            //         _healthSystem.IsDead);
            // }


            _inputMoveProvider.CalculateHorizontalInput();
        }

        private MovementInput zero = new MovementInput() { X = 0 };

        private void FixedUpdate()
        {
            _mover.RunGroundCheck();

            _mover.CalculateHorizontalSpeed(_inputMoveProvider.Input, _movementData);
            _mover.CalculateJump(_inputMoveProvider.Input, _movementData, _enduranceSystem);

            AttackInfo? info = null;
            _attacker.UpdateRechargeTimer(_attacksData);
            // int activeAttackIndex = _inputFightingInputProvider.ActiveAttackIndex;
            // if (activeAttackIndex != -1 && _attacker.CanPerformAttack(activeAttackIndex))
            // {
            //     _attacker.Attack(_inputFightingInputProvider.ActiveAttackIndex, _attacksData);
            //     info = _attacksData.Attacks[_inputFightingInputProvider.ActiveAttackIndex];
            // }
            //
            // if (!_attacker.IsAttacking)
            // {
            //     _mover.CalculateHorizontalSpeed(_inputMoveProvider.Input, _movementData);
            // }
            //
            // _animation.UpdateAnimationSystem(_inputMoveProvider.Input, info, _mover.Velocity, _mover.IsGrounded,
            //     _healthSystem.IsDead);

            _inputMoveProvider.ResetOneTimeActions();
            //_inputFightingInputProvider.ResetAttackIndex(_inputFightingInputProvider.ActiveAttackIndex);
        }

        private void OnAnimationPerformed(PlayerAnimationState animationState)
        {
            switch (animationState)
            {
                case PlayerAnimationState.Attack:
                    Debug.Log("I am attacking");
                    return;
            }
        }
    }
}