using System;
using System.Collections.Generic;
using Items.Core;
using UnityEngine;

namespace Core.PotionSystem
{
    public class PotionSystem : MonoBehaviour
    {
        [SerializeField] private GameObject optionPrefab;
        [SerializeField] private Transform body;
        private List<PotionOption> _options;
        private int _potionCount;
        public event Action<int> OnOptionSelected;
        public event Action OnActive;
        
        public void Setup(List<Potion> potions)
        {
            _options = new List<PotionOption>();
            foreach (var potion in potions)
            {
                GameObject potionGameObject = Instantiate(optionPrefab, body, true);
                PotionOption option = potionGameObject.GetComponent<PotionOption>();
                option.Setup(potion);
                option.OnSelected += OptionOnSelected;
                _options.Add(option);
            }

            _potionCount = _options.Count;
            gameObject.SetActive(false);
        }

        public void OpenPotionMenu()
        {
            gameObject.SetActive(true);
            if(OnActive != null) OnActive.Invoke();
        }

        private void OptionOnSelected(PotionOption sender)
        {
            _options.Remove(sender);
            Destroy(sender.gameObject);
            
            gameObject.SetActive(false);

            if(OnOptionSelected != null) OnOptionSelected.Invoke(_potionCount-_options.Count);
        }

    }
}