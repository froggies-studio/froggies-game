using System.Collections.Generic;
using System.Linq;
using Items.Data;
using Items.Enum;
using Movement;
using UnityEngine;

namespace Items
{
    public class DropGenerator
    {
        private readonly DirectionalMover _entity;
        private readonly ItemSystem _itemSystem;
        private readonly List<ItemDescriptor> _itemDescriptors;
        
        public DropGenerator(DirectionalMover entity, ItemSystem itemSystem, List<ItemDescriptor> itemDescriptors)
        {
            _entity = entity;
            _itemSystem = itemSystem;
            _itemSystem.ItemDestroyed += AddItemDescriptor;
            _itemDescriptors = itemDescriptors;
        }

        public void DropRandomItemWithChance(ItemRarity rarity, float chance)
        {
            if (Random.Range(0, 1) <= chance)
            {
                DropRandomItem(rarity);
            }
        }

        public void DropRandomItem(ItemRarity rarity)
        {
            var items = _itemDescriptors.Where(item => item.ItemRarity <= rarity).ToList();
            if (items.Count == 0)
                return;
            
            var itemDescriptor =  items[Random.Range(0, items.Count)];
            
            if(itemDescriptor.ItemId != ItemId.PowerPotion)
                _itemDescriptors.Remove(itemDescriptor);
            
            _itemSystem.DropItem(itemDescriptor, (Vector2)_entity.transform.position + Vector2.one);
        }
        
        private ItemRarity GetDropRarity()
        {
            float chance = Random.Range(0, 100);
            return chance switch
            {
                <= 25 => ItemRarity.Trash,
                > 25 and <= 50 => ItemRarity.Common,
                > 50 and <= 70 => ItemRarity.Uncommon,
                > 70 and <= 85 => ItemRarity.Rare,
                > 85 and <= 95 => ItemRarity.Epic,
                > 95 and <= 100 => ItemRarity.Legendary,
                _ => ItemRarity.Trash
            };
        }
        
        private void AddItemDescriptor(ItemDescriptor descriptor)
        {
            _itemDescriptors.Add(descriptor);
        }
        
        public void Update() // TODO: remove
        {
            if (Input.GetKeyUp(KeyCode.G))
            {
                DropRandomItem(GetDropRarity());
            }
        }
    }
}