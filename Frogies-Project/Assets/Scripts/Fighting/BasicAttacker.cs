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

        private float _attackRechargeTimer;
        private int _activeAttackIndex = -1;

        public BasicAttacker(EnduranceSystem enduranceSystem, 
            LayerMask attackLayerMask, 
            Collider2D[] attackColliders,
            AttacksData attacksData)
        {
            _enduranceSystem = enduranceSystem;
            _attackContactFilter = new ContactFilter2D();
            _attackContactFilter.SetLayerMask(attackLayerMask);
            _attackColliders = attackColliders;
            _attacksData = attacksData;
        }

        public AttackInfo GetActiveAttackInfo()
        {
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
            var size = Physics2D.OverlapCollider(_attackColliders[_activeAttackIndex], _attackContactFilter,
                _attackTargetsBuffer);
            for (int i = 0; i < size; i++)
            {
                var target = _attackTargetsBuffer[i].GetComponent<DamageReceiver>();
                if (target != null)
                {
                    target.ReceiveDamage(_attacksData.Attacks[_activeAttackIndex].damageAmount);
                }
            }
        }

        public void SetActiveAttackIndex(int activeAttackIndex, StatsController statsController)
        {
            _activeAttackIndex = activeAttackIndex;
            _attackRechargeTimer = statsController.GetStatsValue(StatType.AttackRecharge)*(_activeAttackIndex+1);
            _enduranceSystem.UseEndurance(_attacksData.Attacks[_activeAttackIndex].enduranceCost);
        }

        public void ResetActiveAttackIndex()
        {
            _activeAttackIndex = -1;
        }
    }
}