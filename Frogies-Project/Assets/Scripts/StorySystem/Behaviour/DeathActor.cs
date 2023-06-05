using System;
using Animation;
using UnityEngine;

namespace StorySystem.Behaviour
{
    public class DeathActor : MonoBehaviour
    {
        [SerializeField] private GameObject actorObject;
        [SerializeField] private AnimationStateManager actorAnimator;

        private PlayerActor _playerActor;
        private StoryDirector _storyDirector;
        public Action deathDialogfinished;
        public void Init(PlayerActor playerActor, StoryDirector storyDirector)
        {
            _playerActor = playerActor;
            _storyDirector = storyDirector;
        }
        public void SpawnDeath()
        {
            var deathActor = Instantiate(actorObject);
            deathActor.transform.position = _playerActor.transform.position;
            actorObject.GetComponent<SimpleStoryTrigger>().SpawnActor(_playerActor, _storyDirector);
        }

        public void HideDeath()
        {
            actorAnimator.TriggerAnimationState(PlayerAnimationState.Death);
            Destroy(actorObject);
            deathDialogfinished.Invoke();
        }
    }
}