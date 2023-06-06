using Animation;
using UnityEngine;

namespace StorySystem.Behaviour
{
    public class ActorSpawnerDataComponent : MonoBehaviour
    {
        [SerializeField] private ActorSpawnerData data;

        public ActorSpawnerData Data => data;
    }
}