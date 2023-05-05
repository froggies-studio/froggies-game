﻿using System;
using Items.Enum;
using UnityEngine;

namespace Items.Data
{
    [Serializable ]
    public class ItemDescriptor
    {
        [field: SerializeField] public ItemId ItemId { get; private set; }
        [field: SerializeField] public ItemMaterial Material { get; private set; }
        [field: SerializeField] public ItemType Type { get; private set; }
        [field: SerializeField] public Sprite ItemSprite { get; private set; }
        [field: SerializeField] public ItemRarity ItemRarity { get; private set; }
        [field: SerializeField] public float Price { get; private set; }

        public ItemDescriptor(ItemId itemId, ItemMaterial material, ItemType type, 
            Sprite itemSprite, ItemRarity itemRarity, float price)
        {
            ItemId = itemId;
            Material = material;
            Type = type;
            ItemSprite = itemSprite;
            ItemRarity = itemRarity;
            Price = price;
        }
    }
}