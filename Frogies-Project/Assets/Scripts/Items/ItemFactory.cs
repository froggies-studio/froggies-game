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
                            throw new NullReferenceException("Item type is not supported");
                    }
                default:
                    throw new NullReferenceException("Item type is not supported");
            }
        }
    }
}