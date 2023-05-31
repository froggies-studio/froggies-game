using System;
using System.Collections.Generic;
using UnityEngine;
using WaveSystem.Enum;

namespace WaveSystem
{
    [Serializable]
    public class Wave
    {
        [field: SerializeField] public int Difficulty { get; private set; }
        [field: SerializeField] public List<EnemyTypeCounter> EnemyTypeCounter { get; private set; }
        [field: SerializeField] public int MaxAmountOfEnemies{ get; private set; }

        public Wave(int difficulty, List<EnemyTypeCounter> enemyTypeCounter)
        {
            Difficulty = difficulty;
            EnemyTypeCounter = enemyTypeCounter;
            foreach (var enemyType in EnemyTypeCounter)
            {
                MaxAmountOfEnemies += enemyType.Amount;   
            }
        }

        public Wave GetCopy() => new(Difficulty, EnemyTypeCounter);
    }
}