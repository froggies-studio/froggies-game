using System;
using Items.Data;

namespace Items.Core
{
    public abstract class ConsumableItem : Item
    {
        public override int Amount => _amount;
        
        private int _amount;
        
        public event Action<int> OnAmountChanged;
        public event Action OnFinished;

        protected ConsumableItem(ItemDescriptor descriptor) : base(descriptor)
        {
            _amount = 1;
        }
        
        public override void Use()
        {
            _amount--;
            OnAmountChanged?.Invoke(_amount);
            
            if(_amount <= 0)
                OnFinished?.Invoke();
        }
        
        public void AddAmount(int amount)
        {
            _amount += amount;
            OnAmountChanged?.Invoke(_amount);
        }
    }
}