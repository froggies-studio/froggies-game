using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StorySystem.Behaviour
{
    public class StoryChoice : MonoBehaviour
    {
        [SerializeField] private TMP_Text choiceLine;
        [SerializeField] private Button choiceButton;

        private bool _isInitialized;
        private int _choiceIndex;
        
        public void InitChoice(string line, int choiceIndex, Action<int> callback)
        {
            Debug.Assert(!_isInitialized);
            
            choiceLine.text = line;
            _choiceIndex = choiceIndex;
            choiceButton.onClick.AddListener(() => callback?.Invoke(_choiceIndex));
        }
    }
}