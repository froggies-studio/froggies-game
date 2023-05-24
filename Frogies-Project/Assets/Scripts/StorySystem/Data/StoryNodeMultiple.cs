using System;
using UnityEngine;

namespace StorySystem.Data
{
    [CreateAssetMenu(fileName = "NewStoryNodeMultiple", menuName = "Data/StoryNodeMultiple")]
    public class StoryNodeMultiple : StoryNode
    {
        [SerializeField] private StoryChoice[] nextNodes;

        public StoryChoice[] NextNodes => nextNodes;
        
        [Serializable]
        public struct StoryChoice
        {
            public StoryLine Line;
            public StoryNode Node;
        }
    }
}