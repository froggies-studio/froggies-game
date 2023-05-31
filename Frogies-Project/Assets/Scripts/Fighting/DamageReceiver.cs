using System;
using Core;
using UnityEngine;

namespace Fighting
{
    public class DamageReceiver : MonoBehaviour
    {
        private Action<float> OnDamageReceived { get; set; }

        public void Initialize(Action<float> onDamageReceived)
        {
            OnDamageReceived += onDamageReceived;
        }

        public void ReceiveDamage(float damage)
        {
             if (GlobalSceneManager.Instance.PlayerData.DirectionalMover.IsDashing)
             {
                  return;
             }
             OnDamageReceived?.Invoke(damage);
        }
    }
}