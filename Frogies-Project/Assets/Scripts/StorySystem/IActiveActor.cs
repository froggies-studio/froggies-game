using StorySystem.Data;

namespace StorySystem.Behaviour
{
    public interface IActiveActor
    {
        void Act(StoryNodeSingle);
    }
}