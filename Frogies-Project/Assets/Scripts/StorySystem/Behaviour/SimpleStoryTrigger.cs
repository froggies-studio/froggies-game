using System;
using StorySystem.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace StorySystem.Behaviour
{
    public class SimpleStoryTrigger : MonoBehaviour
    {
        [SerializeField] private StoryActor actor;
        [SerializeField] private StoryNode startNode;

        private StoryDirector _director;
        private PlayerActor _playerActor;
        private bool _isInitialized;
        private bool _isTriggered;

        public void InitTrigger(StoryDirector director, PlayerActor playerActor)
        {
            Debug.Assert(!_isInitialized, "Trigger is already initialized");

            _director = director;
            _playerActor = playerActor;

            _isInitialized = true;
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
    }
}