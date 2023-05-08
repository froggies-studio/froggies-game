using System;
using UnityEngine;

namespace Enemies
{
    public class DamageReceiver : MonoBehaviour
    {
        public Action<float> OnDamageReceived { get; private set; }

        public void Initialize(Action<float> onDamageReceived)
        {
            OnDamageReceived += onDamageReceived;
        }

        public void ReceiveDamage(float damage)
        {
            Debug.Log("Damage received");
            OnDamageReceived?.Invoke(damage);
        }
    }
}