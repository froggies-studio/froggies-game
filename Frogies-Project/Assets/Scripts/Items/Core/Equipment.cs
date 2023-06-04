using System;
using Items.Data;
using Items.Enum;
using StatsSystem;

namespace Items.Core
{
    public class Equipment : Item
    {
        private bool _equipped;
        
        public EquipmentType EquipmentType { get; }
        public StatsController StatsController { get; }

        public Equipment(StatChangingItemDescriptor descriptor, StatsController statsController) : base(descriptor)
        {
            StatsController = statsController;
            if (descriptor.Type == ItemType.Weapon)
                EquipmentType = EquipmentType.Weapon;
        }

        public override int Amount => -1;

        public override void Use()
        {
            _equipped = true;
            foreach (var modifier in ((StatChangingItemDescriptor)Descriptor).StatModifiers)
            {
                StatsController.ProcessModifier(modifier);
            }
        }
    }
}