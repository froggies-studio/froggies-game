﻿using System;
using StatsSystem.Enum;
using UnityEngine;

namespace StatsSystem.Health
{
    public class HealthSystem
    {
        public event EventHandler OnHealthChanged;
        public event EventHandler OnDead;
        
        private StatsController _statsController;

        public HealthSystem(StatsController statsController)
        {
            _statsController = statsController;
        }

        public float GetHealth() => _statsController.GetStatsValue(StatType.Health);
        public float GetHealthPercent() =>  GetHealth() / _statsController.GetStatsValue(StatType.MaxHealth);
        public bool IsDead => GetHealth() <= 0;
        
        public void TakeDamage(float damage)
        {
            float maxDamage = GetHealth();
            damage = maxDamage > damage ? damage : maxDamage;
            
            _statsController.ProcessModifier(new StatModifier(
                new Stat(StatType.Health, -damage), StatModificatorType.Additive, -1, Time.time));
            
            if(OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
            if (damage >= maxDamage && OnDead != null) OnDead(this, EventArgs.Empty);
        }

        public void Heal(float healAmount)
        {
            float maxHealAmount = _statsController.GetStatsValue(StatType.MaxHealth) - GetHealth();
            healAmount = maxHealAmount > healAmount ? healAmount : maxHealAmount;
            
            _statsController.ProcessModifier(new StatModifier(
                new Stat(StatType.Health, healAmount), StatModificatorType.Additive, -1, Time.time));
            
            if(OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
        }

        public void Scale(float scale)
        {
            _statsController.ProcessModifier(new StatModifier(
                new Stat(StatType.MaxHealth, scale), StatModificatorType.Multiplier, -1, Time.time));
            
            _statsController.ProcessModifier(new StatModifier(
                new Stat(StatType.Health, scale), StatModificatorType.Multiplier, -1, Time.time));
            
            if(OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
        }
    }
}