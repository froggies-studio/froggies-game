using System;
using Core;
using StatsSystem.Enum;
using UnityEngine;

namespace StatsSystem.Health
{
    public class HealthBar : MonoBehaviour
    {
        private StatsController _statsController;
        
        public void Setup(StatsController statsController)
        {
            _statsController = statsController;
            _statsController.OnStatChanged += StatsController_OnStatChanged;
        }

        private void StatsController_OnStatChanged(Stat stat)
        {
            if (stat.Type == StatType.Health)
            {
                float percent = stat.Value / _statsController.GetStatsValue(StatType.MaxHealth);
                transform.Find("Scale").localScale = new Vector3(percent, 1);

            }
        }
    }
}