using StorySystem.Behaviour;
using StorySystem.Data;
using UnityEngine;

namespace StorySystem
{
    public class StoryDirector
    {
        private bool _isRunning;
        private bool _isActing;
        private bool _isWaitingForChoice;

        private StoryNode _activeNode;
        private StoryActor _currentActor;
        private StoryActor _actor1;
        private StoryActor _actor2;
        private IActiveActor _activeActor;
        
        public void StartStory(StoryNode starter, StoryActor actor1, StoryActor actor2, IActiveActor activeActor)
        {
            Debug.Assert(!_isRunning, "Story is already running");
            _activeNode = starter;
            _actor1 = actor1;
            _actor2 = actor2;
            _activeActor = activeActor;
            _isRunning = true;
            _activeActor.ChoiceCallback += OnChoiceCallback;
            
            Act(starter);
        }

        private void Act(StoryNode node)
        {
            switch (node)
            {
                case StoryNodeSingle singleNode:
                    if(singleNode.SwitchActor)
                        _currentActor = _currentActor == _actor1 ? _actor2 : _actor1;
                    _currentActor.Act(node.Line, () => _isActing = false);
                    _isActing = true;
                    break;
                case StoryNodeMultiple multipleNode:
                    _activeActor.Act(multipleNode.NextNodes);
                    break;
            }
            
            _activeNode = node;
        }

        private void OnChoiceCallback(int choiceNum)
        {
            if (_isActing)
            {
                _currentActor.ForceStopActing();
                return;
            }
             
            switch (_activeNode)
            {
                case StoryNodeSingle singleNode:
                    Act(singleNode.NextNode);
                    break;
                case StoryNodeMultiple multipleNode:
                    Act(multipleNode.NextNodes[choiceNum]);
                    break;
            }
        }
    }
}