using System;
using Core;
using StatsSystem.Enum;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatsSystem.Health
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Transform scale;
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
                scale.localScale = new Vector3(percent, 1);
                Debug.Log("Health changed!");
            }
        }
    }
}