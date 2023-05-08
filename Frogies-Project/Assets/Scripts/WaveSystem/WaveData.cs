using System;
using System.Collections.Generic;
using UnityEngine;

namespace WaveSystem
{
    [Serializable]
    public class WaveData
    {
        [field: SerializeField] public List<GameObject> Spawners { get; private set; }
        [field: SerializeField] public List<GameObject> Enemies { get; private set; }
        [field: SerializeField] public WaveBarController WaveBar { get; private set; }
    }
}