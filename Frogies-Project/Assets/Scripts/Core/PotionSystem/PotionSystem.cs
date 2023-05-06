using System;
using System.Collections.Generic;
using Items.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Core.PotionSystem
{
    public class PotionSystem : MonoBehaviour
    {
        [SerializeField] private GameObject optionPrefab;
        [SerializeField] private Transform body;
        private List<PotionOption> _options;
        public event EventHandler OnOptionSelected;
        public event EventHandler OnActive;
        
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
            gameObject.SetActive(false);
        }

        public void OnNewDay() //connect with day/night feature
        {
            gameObject.SetActive(true);
            if(OnActive != null) OnActive.Invoke(this, EventArgs.Empty);
        }

        private void OptionOnSelected(PotionOption sender)
        {
            _options.Remove(sender);
            Destroy(sender.gameObject);
            
            gameObject.SetActive(false);

            if(OnOptionSelected != null) OnOptionSelected.Invoke(this, EventArgs.Empty);
        }

    }
}