using System;
using StatsSystem.Enum;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace StatsSystem.Endurance
{
    public class EnduranceControlBar : MonoBehaviour
    {
        [SerializeField] private Slider scale;
        [SerializeField] private TMP_Text currentEnduranceText;
        private StatsController _statsController;
        
        public void Setup(StatsController statsController)
        {
            _statsController = statsController;
            _statsController.OnStatChanged += StatsController_OnStatChanged;
            currentEnduranceText.text = _statsController.GetStatsValue(StatType.MaxEndurance).ToString();
        }

        private void StatsController_OnStatChanged(Stat stat)
        {
            if (stat.Type == StatType.Endurance)
            {
                float percent = stat.Value / _statsController.GetStatsValue(StatType.MaxEndurance);
                scale.value = percent;
                currentEnduranceText.text = Math.Round(stat.Value).ToString();
            }
        }
    }
}