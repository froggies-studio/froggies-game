using UnityEngine;

namespace StorySystem.Data
{
    public class StoryNode : ScriptableObject
    {
        [SerializeField] private StoryLine line;
        
        public StoryLine Line => line;
    }
}