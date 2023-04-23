using System.Collections.Generic;
using System.Linq;
using StatsSystem.Enum;
using UnityEngine;
using UnityEngine.Assertions;

namespace StatsSystem
{
    public class StatsController : IStatValueGiver
    {
        private readonly Dictionary<Stat, Stat> _currentStats;
        private readonly List<StatModificator> _activeModificators;

        public StatsController(Dictionary<Stat, Stat> currentStats)
        {
            _currentStats = currentStats;
            _activeModificators = new List<StatModificator>();
        }

        public float GetStatsValue(StatType statType) =>
            _currentStats.FirstOrDefault(stat => stat.Key.Type == statType).Key;
        

        public void ProcessModificator(StatModificator statModificator)
        {
            var statToChange = _currentStats.FirstOrDefault(stat => stat.Key.Type == statModificator.Stat.Type).Key;
            Debug.Assert(statToChange!=null);

            var addedValue = statModificator.Type == StatModificatorType.Additive
                ? statToChange + statModificator.Stat
                : statToChange * statModificator.Stat;
            
            statToChange.SetStatValue(addedValue);

            if (statModificator.Duration<0)
            {
                return;
            }

            if (_activeModificators.Contains(statModificator))
            {
                _activeModificators.Remove(statModificator);
            }
            else
            {
                var addedStat = new Stat(statModificator.Stat.Type, addedValue);
                var tempModificator = new StatModificator(addedStat, statModificator.Type,
                    statModificator.Duration, Time.time);
                _activeModificators.Add(tempModificator);
            }
        }

        private void OnUpdate()
        {
            if (_activeModificators.Count == 0 )
            {
                return;
            }

            var expiredModificators =
                _activeModificators.Where(modificator => modificator.StartTime + modificator.Duration >= Time.time);

            foreach (var modificator in expiredModificators)
            {
                ProcessModificator(modificator);
            }
        }
    }
}