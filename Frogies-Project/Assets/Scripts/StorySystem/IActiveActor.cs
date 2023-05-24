using System;
using StorySystem.Behaviour;
using StorySystem.Data;

namespace StorySystem
{
    public interface IActiveActor
    {
        event Action<int> ChoiceCallback;
        void Act(StoryNodeMultiple.StoryChoice[] nodes);
    }
}