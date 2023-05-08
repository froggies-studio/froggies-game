using System;
using System.Collections.Generic;
using Items.Enum;
using StatsSystem;
using UnityEngine;

namespace Items.Data
{
    [Serializable]
    public class StatChangingItemDescriptor : ItemDescriptor
    {
        [field: SerializeField] public List<StatModifier> StatModifiers { get; private set; }
        [field: SerializeField] public String Name;
        [field: SerializeField] public String Description;

        public StatChangingItemDescriptor(
            ItemId itemId, ItemMaterial material, ItemType type, 
            Sprite itemSprite, ItemRarity itemRarity, float price, List<StatModifier> modifiers) : 
            base(itemId, material, type, itemSprite, itemRarity, price)
        {
            StatModifiers = modifiers;
        }
    }
}