using JetBrains.Annotations;
using UnityEngine;

namespace StorySystem.Data
{
    [CreateAssetMenu(fileName = "NewStoryNodeSingle", menuName = "Data/StoryNodeSingle")]
    public class StoryNodeSingle : StoryNode
    {
        [SerializeField] [CanBeNull] private StoryNode nextNode;
        [SerializeField] private bool switchActor = true;
        
        [CanBeNull]
        public StoryNode NextNode => nextNode;

        public bool SwitchActor => switchActor;
    }
}