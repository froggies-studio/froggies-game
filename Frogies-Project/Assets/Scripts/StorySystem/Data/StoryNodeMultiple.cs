using UnityEngine;

namespace StorySystem.Data
{
    [CreateAssetMenu(fileName = "NewStoryNodeMultiple", menuName = "Data/StoryNodeMultiple")]
    public class StoryNodeMultiple : StoryNode
    {
        [SerializeField] private StoryNode[] nextNodes;

        public StoryNode[] NextNodes => nextNodes;
    }
}