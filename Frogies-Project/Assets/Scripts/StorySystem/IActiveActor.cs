using System;
using StorySystem.Data;

namespace StorySystem
{
    public interface IActiveActor
    {
        event Action<int> ChoiceCallback;
        void Act(StoryNode[] nodes);
    }
}