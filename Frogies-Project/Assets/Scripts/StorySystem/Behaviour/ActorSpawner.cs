using System;
using Animation;
using Unity.VisualScripting;
using UnityEngine;

namespace StorySystem.Behaviour
{
    public class ActorSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerActor _playerActor;
        
        private StoryDirector _storyDirector;
        private ActorSpawnerDataComponent _spawnerDataComponent;
        private GameObject currentActor;
        public Action onActorDialogFinished;
        public void Init(StoryDirector storyDirector, ActorSpawnerDataComponent spawnerDataComponent)
        {
            _storyDirector = storyDirector;
            _spawnerDataComponent = spawnerDataComponent;
        }
        public void SpawnActor(Transform spawnPoint)
        {
            currentActor = Instantiate(_spawnerDataComponent.Data.actorPrefab);
            currentActor.transform.position = spawnPoint.position;
            currentActor.GetComponent<SimpleStoryTrigger>().SpawnActor(_playerActor, _storyDirector);
        }

        public void HideActor()
        {
            _spawnerDataComponent.Data.actorAnimator.TriggerAnimationState(PlayerAnimationState.Death);
            Destroy(currentActor);
            onActorDialogFinished.Invoke();
        }
    }
}