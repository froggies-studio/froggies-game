using Items.Data;
using Items.Enum;

namespace Items.Core
{
    public class Equipment : Item
    {
        private bool _equipped;
        
        public EquipmentType EquipmentType { get; }
        
        public Equipment(ItemDescriptor descriptor) : base(descriptor)
        {
        }

        public override int Amount => -1;

        public override void Use()
        {
            //Todo implement with stat system
        }
    }
}