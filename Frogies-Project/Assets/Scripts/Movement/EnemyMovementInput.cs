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

        public void CalculateHorizontalInput(bool attackPerformed)
        {
            Input = new MovementInput
            {
                JumpDown = false,
                JumpUp = false,
                X = attackPerformed 
                    ? 0 
                    : Mathf.Clamp(_playerTransform.position.x - _enemyTransform.position.x,
                        Random.Range(-1f, 0f),
                        Random.Range(0f, 1f))
            };
        }
    }
}