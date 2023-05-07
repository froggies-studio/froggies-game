using System;
using System.Collections.Generic;
using Items.Core;
using Items.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Image armorIcon;
        [SerializeField] private Image weaponIcon;
        [SerializeField] private Button potionUseButton;
        [SerializeField] private TMP_Text potionCountText;
        private Equipment _armor;
        private Equipment _weapon;
        private List<Potion> _potions;

        private void Awake()
        {
            potionUseButton.onClick.AddListener(UsePotion);
            _potions = new List<Potion>();
        }

        public void AddNewItem(Item item)
        {
            switch (item)
            {
                case Equipment equipment:
                    Debug.Log(equipment.EquipmentType);
                    AddNewEquipment(equipment);
                    break;
                case Potion potion:
                    if (potion.Descriptor.ItemId == ItemId.PowerPotion)
                    {
                        _potions.Add(potion);
                        potionCountText.text = _potions.Count.ToString();
                    }
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void AddNewEquipment(Equipment newEquipment)
        {
            switch (newEquipment.EquipmentType)
            {
                case EquipmentType.Body:
                    if (newEquipment.Descriptor.ItemId == ItemId.Breastplate)
                    {
                        _armor = newEquipment;
                        armorIcon.sprite = _armor.Descriptor.ItemSprite;
                    }
                    break;
                case EquipmentType.Weapon:
                    _weapon = newEquipment;
                    weaponIcon.sprite = _weapon.Descriptor.ItemSprite;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        private void UsePotion()
        {
            if (_potions.Count == 0)
                return;

            Potion potion = _potions[0];
            potion.Use();
            _potions.RemoveAt(0);
            potionCountText.text = _potions.Count.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                UsePotion();
            }
        }
    }
}