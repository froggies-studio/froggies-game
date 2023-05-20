using System.Collections.Generic;
using UnityEngine;

namespace StorySystem.Behaviour
{
    public class StoryTriggerManager: MonoBehaviour
    {
        [SerializeField] private List<SimpleStoryTrigger> storyTriggers;

        public void InitTriggers(PlayerActor playerActor, StoryDirector storyDirector)
        {
            foreach (var trigger in storyTriggers)
            {
                trigger.InitTrigger(storyDirector, playerActor);
            }
        }
    }
}