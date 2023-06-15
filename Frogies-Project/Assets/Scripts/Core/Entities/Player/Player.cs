using System;
using System.Linq;
using Fighting;
using Movement;
using StatsSystem;
using StatsSystem.Endurance;
using UnityEngine;

namespace Core.Entities.Player
{
    public class Player : MonoBehaviour, IPlayerController
    {
        [SerializeField] private DirectionalMover directionalMover;
        [SerializeField] private StatsStorage statsStorage;
        [SerializeField] private MovementData movementData;

        public MovementInput MovementInput => _movementInputProvider.Input;
        public AttackInfo? AttackInfo => null;
        public Vector2 Velocity => directionalMover.Velocity;
        public bool IsGrounded => directionalMover.IsGrounded;
        [SerializeField] private bool isGrounded;
        public bool IsRollingOver => directionalMover.IsDashing;
        public bool IsMoving => _movementInputProvider.Input.X != 0;
        public bool IsDead => false;

        private IMovementInputProvider _movementInputProvider;
        private PlayerInputActions _playerInputActions;

        private EnduranceSystem _enduranceSystem;
        private StatsController _statsController;

        private void Start()
        {
            PlayerInputActions playerInputActions = new PlayerInputActions();
            playerInputActions.Player.Enable();

            _movementInputProvider = new PlayerMoveInputReader(playerInputActions);

            var stats = statsStorage.Stats.Select(stat => stat.GetCopy()).ToDictionary(stat => stat);
            _statsController = new StatsController(stats);
            _enduranceSystem = new EnduranceSystem(_statsController);
        }

        private void Update()
        {
            directionalMover.RunGroundCheck();
            isGrounded = directionalMover.IsGrounded;


            directionalMover.CalculateJump(_movementInputProvider.Input, movementData, _enduranceSystem,
                _statsController);

            //directionalMover.CalculateRollOver(_movementInputProvider.Input, movementData, _enduranceSystem);


            directionalMover.CalculateHorizontalSpeed(_movementInputProvider.Input, movementData, _statsController);
            _movementInputProvider.ResetOneTimeActions();
        }
    }
}