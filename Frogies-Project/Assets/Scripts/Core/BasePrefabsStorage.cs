using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "BasePrefabsStorage", menuName = "Basics/BasePrefabsStorage")]
    public class BasePrefabsStorage : ScriptableObject
    {
        [SerializeField] private GameObject _sceneItemPrefab;
        
        public GameObject SceneItemPrefab => _sceneItemPrefab;
    }
}