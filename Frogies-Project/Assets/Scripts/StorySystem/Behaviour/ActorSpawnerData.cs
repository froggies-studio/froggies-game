using Animation;
using UnityEngine;

namespace StorySystem.Behaviour
{
    [CreateAssetMenu]
    public class ActorSpawnerData : ScriptableObject
    {
        [SerializeField] private GameObject playerSpawnPoint;
        [SerializeField] private GameObject actorPrefab;

        public GameObject PlayerSpawnPoint => playerSpawnPoint;
        public GameObject ActorPrefab => actorPrefab;
    }
}