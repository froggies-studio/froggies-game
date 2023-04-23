using System;
using Items.Core;
using Items.Data;
using Items.Enum;

namespace Items
{
    public class ItemFactory
    {
        public Item CreateItem(ItemDescriptor descriptor)
        {
            switch (descriptor.Type)
            {
                case ItemType.Consumable:
                    switch (descriptor.ItemId)
                    {
                        case ItemId.HealthPotion:
                        case ItemId.ManaPotion:
                            return new Potion(descriptor);
                        default:
                            throw new NullReferenceException($"Item ID: '{descriptor.ItemId}' is not supported");
                    }
                case ItemType.Weapon:
                    switch (descriptor.ItemId)
                    {
                        case ItemId.Sword:
                        case ItemId.Shield:
                        case ItemId.Spear:
                            return new Equipment(descriptor);
                        default:
                            throw new NullReferenceException($"Item ID: '{descriptor.ItemId}' is not supported");
                    }
                case ItemType.Armor:
                    switch (descriptor.ItemId)
                    {
                        case ItemId.Helmet:
                        case ItemId.Breastplate:
                        case ItemId.Boots:
                            return new Equipment(descriptor);
                        default:
                            throw new NullReferenceException($"Item ID: '{descriptor.ItemId}' is not supported");
                    }
                case ItemType.Quest:
                    switch (descriptor.ItemId)
                    {
                        case ItemId.QuestItem:
                            return new Equipment(descriptor);
                        default:
                            throw new NullReferenceException($"Item ID: '{descriptor.ItemId}' is not supported");
                    }
                case ItemType.Key:
                    switch (descriptor.ItemId)
                    {
                        case ItemId.Key:
                            return new Equipment(descriptor);
                        default:
                            throw new NullReferenceException($"Item ID: '{descriptor.ItemId}' is not supported");
                    }
                case ItemType.Misc:
                    switch (descriptor.ItemId)
                    {
                        case ItemId.Coin:
                        case ItemId.Food:
                            return new Equipment(descriptor);
                        default:
                            throw new NullReferenceException($"Item ID: '{descriptor.ItemId}' is not supported");
                    }
                default:
                    throw new NullReferenceException($"Item type: '{descriptor.Type}' is not supported");
            }
        }
    }
}