using UnityEngine;

namespace Fighting
{
    public struct DamageInfo
    {
        private float _damageAmount;
        private KnockbackInfo _knockbackInfo;

        public float DamageAmount => _damageAmount;

        public KnockbackInfo KnockbackInfo => _knockbackInfo;

        public DamageInfo(float damageAmount, KnockbackInfo knockbackInfo)
        {
            _damageAmount = damageAmount;
            _knockbackInfo = knockbackInfo;
        }
    }
}