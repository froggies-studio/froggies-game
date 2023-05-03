using System;
using StatsSystem.Enum;
using TMPro;
using UnityEngine;

namespace StatsSystem.Endurance
{
    public class EnduranceControlBar : MonoBehaviour
    {
        [SerializeField] private Transform _scale;
        [SerializeField] private TMP_Text _currentHealthText;
        private StatsController _statsController;
        
        public void Setup(StatsController statsController)
        {
            _statsController = statsController;
            _statsController.OnStatChanged += StatsController_OnStatChanged;
            _currentHealthText.text = _statsController.GetStatsValue(StatType.MaxEndurance).ToString();
        }

        private void StatsController_OnStatChanged(Stat stat)
        {
            if (stat.Type == StatType.Endurance)
            {
                float percent = stat.Value / _statsController.GetStatsValue(StatType.MaxEndurance);
                _scale.localScale = new Vector3(percent, 1);
                _currentHealthText.text = Math.Round(stat.Value).ToString();
            }
        }
    }
}