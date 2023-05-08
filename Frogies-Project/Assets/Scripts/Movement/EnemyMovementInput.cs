using UnityEngine;

namespace Movement
{
    public class EnemyMovementInput : IMovementInputProvider
    {
        private readonly Transform _playerTransform;
        private readonly Transform _enemyTransform;

        public EnemyMovementInput(Transform playerTransform, Transform enemyTransform)
        {
            _playerTransform = playerTransform;
            _enemyTransform = enemyTransform;
        }

        public MovementInput Input { get; private set; }

        public void ResetOneTimeActions()
        {
            Input = new MovementInput
            {
                JumpDown = false,
                JumpUp = false,
                X = Input.X
            };
        }

        public void CalculateHorizontalInput(bool inAttackRange)
        {
            Input = new MovementInput
            {
                JumpDown = false,
                JumpUp = false,
                X = inAttackRange 
                    ? 0 
                    : Mathf.Clamp(_playerTransform.position.x - _enemyTransform.position.x, -1f, 1f)
            };
        }
    }
}