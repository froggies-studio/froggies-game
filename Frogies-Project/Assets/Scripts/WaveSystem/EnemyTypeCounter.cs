using System;
using UnityEngine;
using WaveSystem.Enum;

namespace WaveSystem
{
    [Serializable]
    public struct EnemyTypeCounter
    {
        public WaveEnemyType EnemyType;
        public int Amount;
        
    }
}