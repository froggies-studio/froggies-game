using System;
using StatsSystem.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StatsSystem.Health
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
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
                slider.value = percent;
                currentHealthText.text = Math.Round(stat.Value).ToString();
            }
        }
    }
}