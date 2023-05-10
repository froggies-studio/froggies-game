using System;
using UnityEngine;
using WaveSystem.Enum;

namespace WaveSystem
{
    [Serializable]
    public class Wave
    {
        [field: SerializeField] public int Difficulty { get; private set; }
        [field: SerializeField] public WaveEnemyType EnemyType { get; private set; }
        [field: SerializeField] public int MaxAmountOfEnemies{ get; private set; }

        public Wave(int difficulty, WaveEnemyType enemyType, int maxAmountOfEnemies)
        {
            Difficulty = difficulty;
            EnemyType = enemyType;
            MaxAmountOfEnemies = maxAmountOfEnemies;
        }

        public Wave GetCopy() => new Wave(Difficulty, EnemyType, MaxAmountOfEnemies);
    }
}