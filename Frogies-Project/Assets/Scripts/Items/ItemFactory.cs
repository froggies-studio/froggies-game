using System;
using Items.Core;
using Items.Data;
using Items.Enum;
using StatsSystem;

namespace Items
{
    public class ItemFactory
    {
        private StatsController _statsController;
        
        public ItemFactory(StatsController statsController)
        {
            _statsController = statsController;
        }
        
        public Item CreateItem(ItemDescriptor descriptor)
        {
            switch (descriptor.Type)
            {
                case ItemType.Consumable:
                    switch (descriptor.ItemId)
                    {
                        case ItemId.DepowerPotion:
                        case ItemId.PowerPotion:
                            return new Potion((StatChangingItemDescriptor)descriptor, _statsController);
                        default:
                            throw new NullReferenceException($"Item ID: '{descriptor.ItemId}' is not supported");
                    }
                case ItemType.Weapon:
                    switch (descriptor.ItemId)
                    {
                        case ItemId.Sword:
                        case ItemId.Shield:
                        case ItemId.Spear:
                            return new Equipment((StatChangingItemDescriptor)descriptor, _statsController);
                        default:
                            throw new NullReferenceException($"Item ID: '{descriptor.ItemId}' is not supported");
                    }
                case ItemType.Armor:
                    switch (descriptor.ItemId)
                    {
                        case ItemId.Helmet:
                        case ItemId.Breastplate:
                        case ItemId.Boots:
                            return new Equipment((StatChangingItemDescriptor)descriptor, _statsController);
                        default:
                            throw new NullReferenceException($"Item ID: '{descriptor.ItemId}' is not supported");
                    }
                case ItemType.Quest:
                    switch (descriptor.ItemId)
                    {
                        case ItemId.QuestItem:
                            return new QuestItem(descriptor);
                        default:
                            throw new NullReferenceException($"Item ID: '{descriptor.ItemId}' is not supported");
                    }
                case ItemType.Key:
                    switch (descriptor.ItemId)
                    {
                        case ItemId.Key:
                            return new QuestItem(descriptor);
                        default:
                            throw new NullReferenceException($"Item ID: '{descriptor.ItemId}' is not supported");
                    }
                case ItemType.Misc:
                    switch (descriptor.ItemId)
                    {
                        case ItemId.Coin:
                        case ItemId.Food:
                        case ItemId.Note:
                            return new QuestItem(descriptor);
                        default:
                            throw new NullReferenceException($"Item ID: '{descriptor.ItemId}' is not supported");
                    }
                default:
                    throw new NullReferenceException($"Item type: '{descriptor.Type}' is not supported");
            }
        }
    }
}