using System;
using StorySystem.Data;
using UnityEngine;
using UnityEngine.UI;

namespace StorySystem.Behaviour
{
    public class PlayerActor : StoryActor, IActiveActor
    {
        [SerializeField] private RectTransform choicesContainer;
        [SerializeField] private Button choicePrefab;
        
        public event Action<int> ChoiceCallback;
        
        private int _choiceNum;
        
        public void Start()
        {
            ClearAct();
        }
        
        public void Act(StoryNode[] nodes)
        {
            _choiceNum = nodes.Length;
            choicesContainer.gameObject.SetActive(true);
            for (var i = 0; i < _choiceNum; i++)
            {
                var choice = Instantiate(choicePrefab.gameObject, choicesContainer);
                int choiceNum = i;
                choice.GetComponent<Button>().onClick.AddListener(() => OnChoiceClicked(choiceNum));
            }
        }

        private void ClearAct()
        {
            foreach (GameObject child in choicesContainer)
            {
                Destroy(child);
            }
            choicesContainer.gameObject.SetActive(false);
        }
        
        private void OnChoiceClicked(int choiceNum)
        {
            Debug.Assert(choiceNum < 0 ||choiceNum < _choiceNum, "Choice number is out of range");
            ClearAct();
            ChoiceCallback?.Invoke(choiceNum);
        }
    }
}