using Items.Data;
using StatsSystem;
using UnityEngine;

namespace Items.Core
{
    public class Potion : ConsumableItem
    {
        private StatsController _statsController;
        public Potion(StatChangingItemDescriptor descriptor, StatsController statsController) : base(descriptor)
        {
            _statsController = statsController;
        }

        public override void Use()
        {
            base.Use();
            foreach (var modifier in ((StatChangingItemDescriptor)Descriptor).StatModifiers)
            {
                _statsController.ProcessModifier(modifier);
            }
        }
    }
}