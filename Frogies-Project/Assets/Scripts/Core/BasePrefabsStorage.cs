using StatsSystem;
using UnityEngine;
using WaveSystem;

namespace Core
{
    [CreateAssetMenu(fileName = "BasePrefabsStorage", menuName = "Basics/BasePrefabsStorage")]
    public class BasePrefabsStorage : ScriptableObject
    {
        [SerializeField] private GameObject _sceneItemPrefab;
        [SerializeField] private StatsStorage _statsStorage;
        [SerializeField] private WaveStorage _waveStorage;
        [SerializeField] private ParticleSystem hitBloodParticlesSystem;
        [SerializeField] private ParticleSystem hitDecalsParticlesSystem;
        
        public GameObject SceneItemPrefab => _sceneItemPrefab;
        public StatsStorage StatsStorage => _statsStorage;
        public WaveStorage WaveStorage => _waveStorage;

        public ParticleSystem HitBloodParticlesSystem => hitBloodParticlesSystem;

        public ParticleSystem HitDecalsParticlesSystem => hitDecalsParticlesSystem;
    }
}