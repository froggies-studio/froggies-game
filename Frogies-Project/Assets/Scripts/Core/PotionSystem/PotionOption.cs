using System;
using Items.Core;
using Items.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.PotionSystem
{
    public class PotionOption : MonoBehaviour
    {
        [SerializeField] private Button useButton;
        [SerializeField] private Image potionIcon;
        [SerializeField] private TMP_Text potionName;
        [SerializeField] private TMP_Text potionDescription;
        public event Action<PotionOption> OnSelected;
        private Potion _potion;

        public void Setup(Potion potion)
        {
            _potion = potion;

            useButton.onClick.AddListener(OnUseButtonClick);

            potionIcon.sprite = potion.Descriptor.ItemSprite;
            potionName.text = ((StatChangingItemDescriptor)potion.Descriptor).Name;
            potionDescription.text = ((StatChangingItemDescriptor)potion.Descriptor).Description;
        }

        private void OnUseButtonClick()
        {
            _potion.Use();
            if (OnSelected != null) OnSelected.Invoke(this);
        }
    }
}
