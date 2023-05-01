using System;
using StatsSystem.Enum;
using TMPro;
using UnityEngine;

namespace StatsSystem.Health
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Transform scale;
        [SerializeField] private TMP_Text currentHealthText;
        private StatsController _statsController;
        
        public void Setup(StatsController statsController)
        {
            _statsController = statsController;
            _statsController.OnStatChanged += StatsController_OnStatChanged;
            currentHealthText.text = _statsController.GetStatsValue(StatType.MaxHealth).ToString();
        }

        private void StatsController_OnStatChanged(Stat stat)
        {
            if (stat.Type == StatType.Health)
            {
                float percent = stat.Value / _statsController.GetStatsValue(StatType.MaxHealth);
                scale.localScale = new Vector3(percent, 1);
                currentHealthText.text = Math.Round(stat.Value).ToString();
            }
        }
    }
}