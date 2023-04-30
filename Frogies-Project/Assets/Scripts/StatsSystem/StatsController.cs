using System;
using System.Collections.Generic;
using System.Linq;
using StatsSystem.Enum;
using UnityEngine;
using UnityEngine.Assertions;

namespace StatsSystem
{
    public class StatsController : IStatValueGiver
    {
        public event Action<Stat> OnStatChanged;
        
        private readonly Dictionary<Stat, Stat> _currentStats;
        private readonly List<StatModifier> _activeModifiers;

        public StatsController(Dictionary<Stat, Stat> currentStats)
        {
            _currentStats = currentStats;
            _activeModifiers = new List<StatModifier>();
        }

        public float GetStatsValue(StatType statType) =>
            _currentStats.FirstOrDefault(stat => stat.Key.Type == statType).Key;
        

        public void ProcessModifier(StatModifier statModifier)
        {
            var statToChange = _currentStats.FirstOrDefault(stat => stat.Key.Type == statModifier.Stat.Type).Key;
            Debug.Assert(statToChange!=null);

            var addedValue = statModifier.Type == StatModificatorType.Additive
                ? statToChange + statModifier.Stat
                : statToChange * statModifier.Stat;
            
            statToChange.SetStatValue(addedValue);

            if (statModifier.Duration<0)
            {
                return;
            }

            if (_activeModifiers.Contains(statModifier))
            {
                _activeModifiers.Remove(statModifier);
            }
            else
            {
                var addedStat = new Stat(statModifier.Stat.Type, addedValue);
                var tempModificator = new StatModifier(addedStat, statModifier.Type,
                    statModifier.Duration, Time.time);
                _activeModifiers.Add(tempModificator);
            }
            
            if (OnStatChanged != null) OnStatChanged.Invoke(statToChange);
        }

        private void OnUpdate()
        {
            if (_activeModifiers.Count == 0 )
            {
                return;
            }

            var expiredModifiers =
                _activeModifiers.Where(modificator => modificator.StartTime + modificator.Duration >= Time.time);

            foreach (var modificator in expiredModifiers)
            {
                ProcessModifier(modificator);
            }
        }
    }
}