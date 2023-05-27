using Items.Data;

namespace Items.Core
{
    public class QuestItem: Item
    {
        public QuestItem(ItemDescriptor descriptor) : base(descriptor)
        {
        }

        public override int Amount { get; }
        public override void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}