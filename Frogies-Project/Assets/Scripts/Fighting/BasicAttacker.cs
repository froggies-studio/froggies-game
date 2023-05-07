using Enemies;
using UnityEngine;
using StatsSystem.Endurance;

namespace Fighting
{
    public class BasicAttacker
    {
        private float _attackRechargeTimer;
        private readonly EnduranceSystem _enduranceSystem;
        private readonly ContactFilter2D _attackContactFilter;
        private readonly Collider2D[] _attackColliders;

        public BasicAttacker(EnduranceSystem enduranceSystem, LayerMask attackLayerMask, Collider2D[] attackColliders)
        {
            _enduranceSystem = enduranceSystem;
            _attackContactFilter = new ContactFilter2D();
            _attackContactFilter.SetLayerMask(attackLayerMask);
            _attackColliders = attackColliders;
        }

        public bool CanPerformAttack(int attackIndex, AttacksData attacksData)
        {
            return attackIndex != -1
                   && _attackRechargeTimer <= 0
                   && _enduranceSystem.CheckEnduranceAbility(attacksData.Attacks[attackIndex].enduranceCost);
        }

        public bool IsAttacking => _attackRechargeTimer > 0;

        public void UpdateRechargeTimer(AttacksData data)
        {
            if (_attackRechargeTimer > 0)
            {
                _attackRechargeTimer -= data.RechargeTimerMultiplayer * Time.deltaTime;
            }
        }

        private const int MaxAttackTargets = 10;
        private Collider2D[] _attackTargetsBuffer = new Collider2D[MaxAttackTargets];

        public void Attack(int index, AttacksData data)
        {
            if (!CanPerformAttack(index,data))
                return;

            if (_attackColliders[index] == null)
            {
                Debug.Log("Attack collider is null");
                return;
            }

            var size = Physics2D.OverlapCollider(_attackColliders[index], _attackContactFilter, _attackTargetsBuffer);
            for (int i = 0; i < size; i++)
            {
                var target = _attackTargetsBuffer[i].GetComponent<BasicEntity>();
                // if (target != null)
                // {
                target.HealthSystem.TakeDamage(data.Attacks[index].damageAmount);
                // }
            }

            _attackRechargeTimer = data.Attacks[index].rechargeTime;
            _enduranceSystem.UseEndurance(data.Attacks[index].enduranceCost);
        }
    }
}