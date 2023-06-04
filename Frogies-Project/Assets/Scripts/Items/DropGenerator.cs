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
            _itemDescriptors = itemDescriptors;
        }

        public void DropRandomItemWithChance(ItemRarity rarity, double chance)
        {
            System.Random random = new System.Random();
            if (random.NextDouble() <= chance)
            {
                DropRandomItem(rarity);
            }
        }

        public void DropRandomItem(ItemRarity rarity)
        {
            var items = _itemDescriptors.Where(item => item.ItemRarity == rarity).ToList();
            var itemDescriptor =  items[Random.Range(0, items.Count)];
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
        
        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.G))
            {
                DropRandomItem(GetDropRarity());
            }
        }
    }
}