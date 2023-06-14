using System;
using Animation;
using UnityEngine.Serialization;

namespace Fighting
{
    [Serializable]
    public struct AttackInfo
    {
        public PlayerAnimationState animationState;
        public float damageAmount;
        public int attackerKnockbackAmount;
        public int receiverKnockbackAmount;
        public float rechargeTime;
        public float enduranceCost;
        public float animationDuration;

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