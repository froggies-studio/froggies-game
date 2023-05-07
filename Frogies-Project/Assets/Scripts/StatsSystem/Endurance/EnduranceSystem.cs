using StatsSystem.Enum;
using UnityEngine;

namespace StatsSystem.Endurance
{ 
    public class EnduranceSystem
    {
        private float _maxEndurance;
        private float _currentEndurance;
        private float _rechargeRate;
        
        private StatsController _statsController;
        
        public EnduranceSystem(StatsController statsController)
        {
            _statsController = statsController;
            _rechargeRate = _statsController.GetStatsValue(StatType.EnduranceRechargeRate);
            _maxEndurance = _statsController.GetStatsValue(StatType.MaxEndurance);
        }
        
        public void UseEndurance(float amountOfEndurance)
        {
            if (CheckEnduranceAbility(amountOfEndurance))
            {
                _statsController.ProcessModifier(new StatModifier(new Stat(StatType.Endurance, -amountOfEndurance), StatModificatorType.Additive, -1, Time.time)); 
            }
        }
        
        public void RestoreEndurance() 
        {
            if (_currentEndurance < _maxEndurance)
            {
                _statsController.ProcessModifier(new StatModifier(new Stat(StatType.Endurance, _rechargeRate*Time.deltaTime), StatModificatorType.Additive, -1, Time.time));

            }
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
