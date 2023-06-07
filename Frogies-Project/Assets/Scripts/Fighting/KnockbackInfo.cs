using UnityEngine;

namespace Fighting
{
    public struct KnockbackInfo
    {
        private int _knockbackAmount;
        
        public int KnockbackAmount => _knockbackAmount;
        public Vector2 KnockbackDirection { get; set; }
        
        public KnockbackInfo(int knockbackAmount)
        {
            _knockbackAmount = knockbackAmount;
            KnockbackDirection = Vector2.zero;
        }
    }
}