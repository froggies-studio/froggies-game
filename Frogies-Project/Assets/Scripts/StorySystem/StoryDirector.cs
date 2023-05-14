using JetBrains.Annotations;
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
            _currentActor = actor1;
            Act(starter);
        }

        private void Act([CanBeNull] StoryNode node)
        {
            if (node == null)
            {
                _isActing = false;
                _isRunning = false;
                _activeActor.ChoiceCallback -= OnChoiceCallback;
                _actor1.Deactivate();
                _actor2.Deactivate();
                return;
            }
            
            switch (node)
            {
                case StoryNodeSingle singleNode:
                    if(singleNode.SwitchActor)
                    {
                        _currentActor.Deactivate();
                        _currentActor = _currentActor == _actor1 ? _actor2 : _actor1;
                    }

                    _isActing = true;
                    _currentActor.Act(node.Line, () => _isActing = false);
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
                    if(choiceNum < 0)
                        return;
                    Act(multipleNode.NextNodes[choiceNum].Node);
                    break;
            }
        }
    }
}