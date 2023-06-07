using UnityEngine;

namespace Fighting
{
    public class EnemyInputFightingProvider : IFightingInputProvider
    {
        public int ActiveAttackIndex { get; private set; } = -1;
        private readonly float _minAttackRange;
        private readonly Transform _playerTransform;
        private readonly Transform _enemyTransform;
        private const float NonMinRangeAttackChance = 0.4f;

        public EnemyInputFightingProvider(float minAttackRange, Transform playerTransform, Transform enemyTransform)
        {
            _minAttackRange = minAttackRange;
            _playerTransform = playerTransform;
            _enemyTransform = enemyTransform;
        }

        public void ResetAttackIndex(int index)
        {
            ActiveAttackIndex = -1;
        }

        public void CalculateAttackInput(bool isInAttackRange)
        {
            if (!isInAttackRange)
            {
                ActiveAttackIndex = -1;
                return;
            }

            float distanceToTarget = Mathf.Abs(_playerTransform.position.x - _enemyTransform.position.x);
            if (distanceToTarget <= _minAttackRange)
            {
                ActiveAttackIndex = 0;
                return;
            }

            ActiveAttackIndex = Random.value < NonMinRangeAttackChance ? 0 : -1;
        }
    }
}