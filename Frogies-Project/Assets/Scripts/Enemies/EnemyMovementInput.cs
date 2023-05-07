using Movement;
using UnityEngine;

namespace Enemies
{
    public class EnemyMovementInput : IMovementInputProvider
    {
        private Transform _playerTransform;
        private Transform _enemyTransform;

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