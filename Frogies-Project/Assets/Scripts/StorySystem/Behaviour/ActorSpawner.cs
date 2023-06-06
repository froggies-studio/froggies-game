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
        private ActorSpawnerData _spawnerData;
        public Action onActorDialogFinished;
        public void Init(StoryDirector storyDirector, ActorSpawnerData spawnerData)
        {
            _storyDirector = storyDirector;
            _spawnerData = spawnerData;
        }
        public void SpawnActor()
        {
            var deathActor = Instantiate(_spawnerData.ActorPrefab);
            deathActor.transform.position = _spawnerData.PlayerSpawnPoint.transform.position;
            deathActor.GetComponent<SimpleStoryTrigger>().SpawnActor(_playerActor, _storyDirector);
        }

        public void HideActor()
        {
            _spawnerData.ActorPrefab.GetComponent<AnimationStateManager>().TriggerAnimationState(PlayerAnimationState.Death);
            Destroy(_spawnerData.ActorPrefab);
            onActorDialogFinished.Invoke();
        }
    }
}