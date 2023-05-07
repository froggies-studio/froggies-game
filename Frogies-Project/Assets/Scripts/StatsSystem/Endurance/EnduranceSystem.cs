using StatsSystem.Enum;
using UnityEngine;

namespace StatsSystem.Endurance
{ 
    public class EnduranceSystem
    {
        private float _currentEndurance;

        private StatsController _statsController;
        
        public EnduranceSystem(StatsController statsController)
        {
            _statsController = statsController;
            _statsController.OnStatChanged += SetMaxEndurance;
        }
        
        public void UseEndurance(float amountOfEndurance)
        {
            if (CheckEnduranceAbility(amountOfEndurance))
            {
                _statsController.ProcessModifier(new StatModifier(new Stat(StatType.Endurance, -amountOfEndurance), StatModificatorType.Additive, -1, Time.time)); 
            }
        }

        public void SetMaxEndurance(Stat stat)
        {
            var maxEndurance = GetMaxEndurance();
            if (stat.Type == StatType.MaxEndurance && stat.Value < maxEndurance)
            {
                _statsController.ProcessModifier(new StatModifier(
                    new Stat(StatType.Endurance,maxEndurance-stat.Value), StatModificatorType.Additive, -1, Time.time));
            }
        }
        
        public void RestoreEndurance()
        {
            var rechargeRate = GetCurrentRechargeRate();
            if (_currentEndurance < GetMaxEndurance())
            {
                _statsController.ProcessModifier(new StatModifier(
                    new Stat(StatType.Endurance, rechargeRate*Time.deltaTime), StatModificatorType.Additive, -1, Time.time));

            }
        }

        public float GetMaxEndurance()
        {
            return _statsController.GetStatsValue(StatType.MaxEndurance);
        }

        public float GetCurrentRechargeRate()
        {
            return _statsController.GetStatsValue(StatType.EnduranceRechargeRate);
        }
        
        public bool CheckEnduranceAbility(float amountOfEndurance)
        {
            if (_currentEndurance - amountOfEndurance<0)
            {
                return false;
            }
        
            return true;
        }
        
        public void SetCurrentEndurance()
        {
            _currentEndurance = _statsController.GetStatsValue(StatType.Endurance); 
        }
    }
}
