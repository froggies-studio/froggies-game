using StorySystem.Data;
using UnityEngine;

namespace StorySystem.Behaviour
{
    public class SimpleStoryTrigger : MonoBehaviour
    {
        [SerializeField] private StoryActor actor;
        [SerializeField] private StoryNode startNode;

        private PlayerActor _playerActor;
        private StoryDirector _director;
        private bool _isInitialized;
        
        public void InitTrigger(StoryDirector director, PlayerActor playerActor)
        {
            Debug.Assert(!_isInitialized, "Trigger is already initialized");

            _director = director;
            _playerActor = playerActor;
            
            _isInitialized = true;
        }
        
        protected void OnMouseDown()
        {
            Debug.Assert(_isInitialized, "Trigger is not initialized");
            
            _director.StartStory(startNode, actor, _playerActor, _playerActor);
        }
    }
}