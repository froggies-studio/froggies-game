using Fighting;
using Movement;
using UnityEngine;

namespace Core.Entities.Player
{
    public class Player : MonoBehaviour, IPlayerController
    {
        [SerializeField] private PlayerAnimator playerAnimator;
        
        public MovementInput MovementInput { get; }
        public AttackInfo? AttackInfo => null;
        public Vector2 Velocity => _currentVelocityDebug;
        public bool IsDead => false;
        public bool IsGrounded => _directionalMover.IsGrounded;
        
        private readonly MovementData _movementData;
        private readonly DirectionalMover _directionalMover;

        public void FixedUpdate()
        {
            // if (HealthSystem.IsDead)
            // {
            //     return;
            // }

            // _directionalMover.RunGroundCheck();

        }

        private Vector2 _currentVelocityDebug;

        public void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            _currentVelocityDebug.x = x;

        }
    }
}