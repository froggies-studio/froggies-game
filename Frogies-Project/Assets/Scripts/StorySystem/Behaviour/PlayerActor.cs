using System;
using Core;
using StorySystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StorySystem.Behaviour
{
    public class PlayerActor : StoryActor, IActiveActor
    {
        [SerializeField] private RectTransform dialogContainer;
        [SerializeField] private RectTransform choicesContainer;
        [SerializeField] private StoryChoice choicePrefab;
        
        public event Action<int> ChoiceCallback;
        
        private int _choiceNum;
        
        public void Init()
        {
            GlobalSceneManager.Instance.PlayerInputActions.Player.NextDialog.performed += ctx => ChoiceCallback?.Invoke(-1);
            ClearAct();
        }
        
        public void Act(StoryNodeMultiple.StoryChoice[] nodes)
        {
            textPanel.gameObject.SetActive(true);
            _choiceNum = nodes.Length;
            dialogContainer.gameObject.SetActive(true);
            for (var i = 0; i < _choiceNum; i++)
            {
                var choice = Instantiate(choicePrefab.gameObject, choicesContainer);
                int choiceNum = i;
                var uiChoice = choice.GetComponent<StoryChoice>();
                uiChoice.InitChoice(nodes[i].Line.Line, choiceNum, OnChoiceClicked);
            }
        }

        private void ClearAct()
        {
            foreach (Transform child in choicesContainer.transform)
            {
                Destroy(child.gameObject);
            }
            dialogContainer.gameObject.SetActive(false);
            Deactivate();
        }
        
        private void OnChoiceClicked(int choiceNum)
        {
            Debug.Assert(choiceNum < 0 ||choiceNum < _choiceNum, "Choice number is out of range");
            ClearAct();
            ChoiceCallback?.Invoke(choiceNum);
        }
    }
}