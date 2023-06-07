using System;
using System.Collections.Generic;
using Animation;
using Core;
using StorySystem.Data;
using UnityEngine;

namespace StorySystem.Behaviour
{
    [Serializable]
    public class ActorSpawnerData
    {
        [field: SerializeField] public GameObject actorPrefab;
        [field: SerializeField] public AnimationStateManager actorAnimator;
        [field: SerializeField] public List<StoryNode> startNodes;
        
        public int _deathStartNodeNumber;

        public void UpdateStartNodeNumber()
        {
            _deathStartNodeNumber = GlobalSceneManager.Instance.PotionSystem._options.Count==5 ? 0 : 1;
        }
    }
}