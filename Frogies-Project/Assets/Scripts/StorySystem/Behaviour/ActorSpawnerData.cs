using System;
using Animation;
using UnityEngine;

namespace StorySystem.Behaviour
{
    [Serializable]
    public class ActorSpawnerData
    {
        [field: SerializeField] public GameObject actorPrefab;
        [field: SerializeField] public AnimationStateManager actorAnimator;
    }
}