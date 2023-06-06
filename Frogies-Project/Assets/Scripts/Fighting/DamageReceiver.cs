using System;
using Core;
using UnityEngine;

namespace Fighting
{
    public class DamageReceiver : MonoBehaviour
    {
        private Action<DamageInfo> OnDamageReceived { get; set; }

        public void Initialize(Action<DamageInfo> onDamageReceived)
        {
            OnDamageReceived += onDamageReceived;
        }

        public void ReceiveDamage(DamageInfo damage)
        {
             if (GlobalSceneManager.Instance.PlayerData.DirectionalMover.IsDashing)
                 return;
             
             OnDamageReceived?.Invoke(damage);
        }
    }
}