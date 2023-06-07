using UnityEngine;

namespace Movement
{
    public class EnemyMovementInput : IMovementInputProvider
    {
        private readonly Transform _playerTransform;
        private readonly Transform _enemyTransform;

        private int _framesWithoutMovement = 0;
        private Vector3 _previousPosition;
        private readonly int _maxFramesWithoutMovement = 5;
        private readonly int _noMovementAfterAttack = 5;

        public EnemyMovementInput(Transform playerTransform, Transform enemyTransform)
        {
            _playerTransform = playerTransform;
            _enemyTransform = enemyTransform;
            _previousPosition = _enemyTransform.position;
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
                JumpDown = _framesWithoutMovement > _maxFramesWithoutMovement,
                JumpUp = _framesWithoutMovement > _maxFramesWithoutMovement,
                X = attackPerformed 
                    ? 0 
                    : Mathf.Clamp(_playerTransform.position.x - _enemyTransform.position.x,
                        Random.Range(-1f, 0f),
                        Random.Range(0f, 1f))
            };
            
            if (attackPerformed)
            {
                _framesWithoutMovement = -1 * _noMovementAfterAttack;
                _previousPosition = _enemyTransform.position;
            }
            else if (_enemyTransform.position == _previousPosition)
            {
                _framesWithoutMovement++;
            }
            else
            {
                _framesWithoutMovement = 0;
                _previousPosition = _enemyTransform.position;
            }
        }
    }
}