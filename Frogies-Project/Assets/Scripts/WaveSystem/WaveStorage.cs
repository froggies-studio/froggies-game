using System.Collections.Generic;

using StatsSystem;
using UnityEngine;

namespace WaveSystem
{
    [CreateAssetMenu(fileName = "WaveStorage", menuName = "Data/Wave")]
    public class WaveStorage : ScriptableObject
    {
        [field: SerializeField] public List<Wave> Waves { get; private set; }
        
    }
}