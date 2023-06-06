using Animation;
using StorySystem.Data;
using UnityEngine;

namespace StorySystem.Behaviour
{
    public class SimpleStoryTrigger : MonoBehaviour
    {
        [SerializeField] private StoryActor actor;
        [SerializeField] private StoryNode startNode;
        [SerializeField] private bool autoTrigger;

        private PlayerActor _playerActor;
        private StoryDirector _director;
        [SerializeField] private bool _isInitialized;
        [SerializeField] private bool _isTriggered;

        public void InitTrigger(StoryDirector director, PlayerActor playerActor)
        {
            Debug.Assert(!_isInitialized, "Trigger is already initialized");
            _isTriggered = false;
            _director = director;
            _playerActor = playerActor;

            _isInitialized = true;
            if (autoTrigger) OnMouseDown();
        }

        private void OnMouseDown()
        {
            Debug.Assert(_isInitialized, "Trigger is not initialized");
            
            if (_isTriggered)
            {
                return;
            }
            
            _isTriggered = true;
            _director.StartStory(startNode, actor, _playerActor, _playerActor);
        }

        public void SpawnActor(PlayerActor playerActor, StoryDirector storyDirector)
        {
            InitTrigger(storyDirector, playerActor);
        }
    }
}