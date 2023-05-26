using System;
using Animation;

namespace Fighting
{
    [Serializable]
    public struct AttackInfo
    {
        public PlayerAnimationState animationState;
        public float damageAmount;
        public float enduranceCost;

        public bool Equals(AttackInfo other)
        {
            return animationState == other.animationState && damageAmount.Equals(other.damageAmount);
        }

        public override bool Equals(object obj)
        {
            return obj is AttackInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) animationState, damageAmount);
        }
    }
}