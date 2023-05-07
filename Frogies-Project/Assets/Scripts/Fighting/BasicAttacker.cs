using UnityEngine;
using StatsSystem.Endurance;

namespace Fighting
{
    public class BasicAttacker
    {
        // private float _weakAttackEndurance = 3;
        // private float _hardAttackEndurance = 5;
        private float _attackRechargeTimer;
        private float[] _attackEndurance = new[] { 3f, 5f };
        private readonly EnduranceSystem _enduranceSystem;

        public BasicAttacker(EnduranceSystem enduranceSystem)
        {
            _enduranceSystem = enduranceSystem;
        }

        public bool CanPerformAttack(int attackIndex)
        {
            return attackIndex != -1 
                   && _attackRechargeTimer <= 0 
                   && _enduranceSystem.CheckEnduranceAbility(_attackEndurance[attackIndex]);
        }

        public bool IsAttacking => _attackRechargeTimer > 0;

        public void UpdateRechargeTimer(AttacksData data)
        {
            if (_attackRechargeTimer > 0)
            {
                _attackRechargeTimer -= data.RechargeTimerMultiplayer * Time.deltaTime;
            }
        }

        public void Attack(int index, AttacksData data)
        {
            if (!CanPerformAttack(index))
                return;

            _attackRechargeTimer = data.Attacks[index].rechargeTime;
            _enduranceSystem.UseEndurance(_attackEndurance[index]);
        }
    }
}