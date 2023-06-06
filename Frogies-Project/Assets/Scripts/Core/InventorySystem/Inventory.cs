using System;
using System.Collections.Generic;
using Items.Core;
using Items.Data;
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
        [SerializeField] private StatChangingItemDescriptor weaponDescriptor;
        [SerializeField] private StatChangingItemDescriptor armorDescriptor;
        
        private List<Potion> _potions;

        private void Awake()
        {
            armorIcon.sprite = armorDescriptor.ItemSprite;
            weaponIcon.sprite = weaponDescriptor.ItemSprite;
            potionUseButton.onClick.AddListener(UsePotion);
            _potions = new List<Potion>();
        }

        public Item AddNewItem(Item item)
        {
            switch (item)
            {
                case Equipment equipment:
                    return AddNewEquipment(equipment);
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

            return null;
        }

        private Item AddNewEquipment(Equipment newEquipment)
        {
            switch (newEquipment.EquipmentType)
            {
                case EquipmentType.Body:
                    if (newEquipment.Descriptor.ItemId == ItemId.Breastplate)
                    {
                        Equipment oldArmor = new Equipment(armorDescriptor, newEquipment.StatsController);
                        armorDescriptor = (StatChangingItemDescriptor)newEquipment.Descriptor;
                        armorIcon.sprite = armorDescriptor.ItemSprite;
                        newEquipment.Use();
                        return oldArmor;
                    }
                    break;
                case EquipmentType.Weapon:
                    Item oldWeapon = new Equipment(weaponDescriptor, newEquipment.StatsController);
                    weaponDescriptor = (StatChangingItemDescriptor)newEquipment.Descriptor;
                    weaponIcon.sprite = weaponDescriptor.ItemSprite;
                    newEquipment.Use();
                    return oldWeapon;
                default:
                    throw new NotSupportedException();
            }

            return null;
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