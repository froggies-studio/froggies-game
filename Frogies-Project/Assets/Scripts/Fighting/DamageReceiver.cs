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
             if (GlobalSceneManager.Instance.PlayerData.DirectionalMover.IsDashing) //if player is dashing he won't take damage
             {
                  return;
             }
             OnDamageReceived?.Invoke(damage);
        }
    }
}