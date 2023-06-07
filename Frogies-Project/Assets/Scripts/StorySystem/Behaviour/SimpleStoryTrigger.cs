using Animation;
using JetBrains.Annotations;
using StorySystem.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace StorySystem.Behaviour
{
    public class SimpleStoryTrigger : MonoBehaviour
    {
        [SerializeField] private StoryActor actor;
        [SerializeField] private bool autoTrigger;
        [SerializeField] [CanBeNull] private StoryNode startNode;
        
        private PlayerActor _playerActor;
        private StoryDirector _director;
        private bool _isInitialized;
        private bool _isTriggered;

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

        public void SpawnActor(PlayerActor playerActor, StoryDirector storyDirector, StoryNode startNode)
        {
            this.startNode = startNode;
            InitTrigger(storyDirector, playerActor);
        }
    }
}