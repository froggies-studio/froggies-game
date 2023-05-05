using StatsSystem.Endurance;
using UnityEngine;

namespace Fighting
{
    public class BasicAttacker
    {
        private float _weakAttackEndurance = 3;
        private float _hardAttackEndurance = 5;
        private float _attackRechargeTimer;

        public bool IsAbleToAttack => _attackRechargeTimer <= 0;
        
        public void UpdateRechargeTimer(AttacksData data)
        {
            if (_attackRechargeTimer > 0)
            {
                _attackRechargeTimer -= data.RechargeTimerMultiplayer * Time.deltaTime;
                return;
            }
        }
        
        public void Attack(int index, AttacksData data, EnduranceSystem enduranceSystem)
        {
            if(!IsAbleToAttack)
                return;

            switch (index)
            {
                case 0:
                    if (!enduranceSystem.CheckEnduranceAbility(_weakAttackEndurance))
                    {
                        return;
                    }
                    enduranceSystem.UseEndurance(_weakAttackEndurance);
                    break;
                    
                case 1:
                    if (!enduranceSystem.CheckEnduranceAbility(_hardAttackEndurance))
                    {
                        return;
                    }
                    enduranceSystem.UseEndurance(_hardAttackEndurance);
                    break;
            }
            
            _attackRechargeTimer = data.Attacks[index].rechargeTime;
        }
    }
}