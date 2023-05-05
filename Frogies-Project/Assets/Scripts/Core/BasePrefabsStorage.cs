using StatsSystem;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "BasePrefabsStorage", menuName = "Basics/BasePrefabsStorage")]
    public class BasePrefabsStorage : ScriptableObject
    {
        [SerializeField] private GameObject _sceneItemPrefab;
        [SerializeField] private StatsStorage _statsStorage;
        
        public GameObject SceneItemPrefab => _sceneItemPrefab;
        public StatsStorage StatsStorage => _statsStorage;
    }
}