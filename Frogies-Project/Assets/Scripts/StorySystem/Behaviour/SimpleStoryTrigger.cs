using System;
using StorySystem.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace StorySystem.Behaviour
{
    public class SimpleStoryTrigger : MonoBehaviour
    {
        [SerializeField] private StoryActor actor;
        [SerializeField] protected StoryNode startNode;

        private PlayerActor _playerActor;
        protected StoryDirector Director;
        protected bool IsInitialized;
        
        public void InitTrigger(StoryDirector director, PlayerActor playerActor)
        {
            Debug.Assert(!IsInitialized, "Trigger is already initialized");

            Director = director;
            _playerActor = playerActor;
            
            IsInitialized = true;
        }
        
        protected void OnMouseDown()
        {
            Debug.Assert(IsInitialized, "Trigger is not initialized");
            
            Director.StartStory(startNode, actor, _playerActor, _playerActor);
        }
    }
}