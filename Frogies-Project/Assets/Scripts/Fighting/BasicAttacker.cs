using System;
using StatsSystem;
using UnityEngine;
using StatsSystem.Endurance;
using StatsSystem.Enum;

namespace Fighting
{
    public class BasicAttacker
    {
        public bool IsAttacking => _attackRechargeTimer > 0;

        private const int MaxAttackTargets = 10;

        private readonly Collider2D[] _attackTargetsBuffer = new Collider2D[MaxAttackTargets];
        private readonly EnduranceSystem _enduranceSystem;
        private readonly ContactFilter2D _attackContactFilter;
        private readonly Collider2D[] _attackColliders;
        private readonly AttacksData _attacksData;
        private readonly Transform _entityTransform;

        private float _attackRechargeTimer;
        private int _activeAttackIndex = -1;

        public event Action<AttackInfo, KnockbackInfo> AttackPerformed;
        
        public BasicAttacker(EnduranceSystem enduranceSystem,
            LayerMask attackLayerMask,
            Collider2D[] attackColliders,
            AttacksData attacksData, Transform entityTransform)
        {
            _enduranceSystem = enduranceSystem;
            _attackContactFilter = new ContactFilter2D();
            _attackContactFilter.SetLayerMask(attackLayerMask);
            _attackColliders = attackColliders;
            _attacksData = attacksData;
            _entityTransform = entityTransform;
        }

        public AttackInfo UpdateAndGetActiveAttackInfo(StatsController statsController)
        {
            _attacksData.Attacks[_activeAttackIndex].rechargeTime =
                statsController.GetStatsValue(StatType.AttackRecharge) * (_activeAttackIndex + 1);
            _attacksData.Attacks[_activeAttackIndex].damageAmount =
                statsController.GetStatsValue(StatType.Damage) * (_activeAttackIndex + 1);
            return _attacksData.Attacks[_activeAttackIndex];
        }

        public bool CanPerformAttack(int attackIndex)
        {
            return attackIndex != -1
                   && _attackRechargeTimer <= 0
                   && _enduranceSystem.CheckEnduranceAbility(_attacksData.Attacks[attackIndex].enduranceCost);
        }

        public void UpdateRechargeTimer()
        {
            if (_attackRechargeTimer > 0)
            {
                _attackRechargeTimer -= _attacksData.RechargeTimerMultiplayer * Time.deltaTime;
            }
        }

        public void Attack()
        {
            _enduranceSystem.UseEndurance(_attacksData.Attacks[_activeAttackIndex].enduranceCost);
            var size = Physics2D.OverlapCollider(_attackColliders[_activeAttackIndex], _attackContactFilter,
                _attackTargetsBuffer);
            
            for (int i = 0; i < size; i++)
            {
                var target = _attackTargetsBuffer[i].GetComponent<DamageReceiver>();
                
                if (target != null)
                {
                    KnockbackInfo knockbackInfo = new KnockbackInfo(_attacksData.Attacks[_activeAttackIndex].receiverKnockbackAmount);
                    knockbackInfo.KnockbackDirection = (target.transform.position - _entityTransform.transform.position).normalized;
                    
                    DamageInfo damageInfo = new DamageInfo(_attacksData.Attacks[_activeAttackIndex].damageAmount, knockbackInfo);
                    target.ReceiveDamage(damageInfo);
                }
            }
            
            KnockbackInfo attackerKnockbackInfo = new KnockbackInfo(_attacksData.Attacks[_activeAttackIndex].attackerKnockbackAmount);
            attackerKnockbackInfo.KnockbackDirection = (_entityTransform.transform.position - _attackColliders[_activeAttackIndex].transform.position);
            AttackPerformed?.Invoke(_attacksData.Attacks[_activeAttackIndex], attackerKnockbackInfo);
        }

        public void SetActiveAttackIndex(int activeAttackIndex)
        {
            _activeAttackIndex = activeAttackIndex;
            _attackRechargeTimer = _attacksData.Attacks[_activeAttackIndex].rechargeTime;
        }

        public void ResetActiveAttackIndex()
        {
            _activeAttackIndex = -1;
        }
    }
}