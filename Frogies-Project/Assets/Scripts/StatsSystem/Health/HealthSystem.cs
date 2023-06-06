using System;
using Fighting;
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
            _statsController.OnStatChanged += StatsController_OnStatChanged;
        }

        public float GetHealth() => _statsController.GetStatsValue(StatType.Health);
        public float GetMaxHealth() => _statsController.GetStatsValue(StatType.MaxHealth);
        public float GetHealthPercent() =>  GetHealth() / _statsController.GetStatsValue(StatType.MaxHealth);
        public bool IsDead => GetHealth() <= 0;
        
        public void TakeDamage(DamageInfo damageInfo)
        {
            float maxDamage = GetHealth();
            float damage = damageInfo.DamageAmount - _statsController.GetStatsValue(StatType.DamageResistance);
            damage = Math.Clamp(damage, 0, maxDamage);

            _statsController.ProcessModifier(new StatModifier(
                new Stat(StatType.Health, -damage), StatModificatorType.Additive, -1, Time.time));
            
            if(OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
            if (damage >= maxDamage && OnDead != null) 
                OnDead(this, EventArgs.Empty);
        }

        public void Heal(float healAmount)
        {
            float maxHealAmount = GetMaxHealth() - GetHealth();
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
        
        private void StatsController_OnStatChanged(Stat stat)
        {
            if (stat.Type == StatType.Health)
            {
                float maxHealth = GetMaxHealth();
                if (stat.Value > maxHealth)
                {
                    _statsController.ProcessModifier(new StatModifier(
                        new Stat(StatType.Health, maxHealth-stat.Value), StatModificatorType.Additive, -1, Time.time));
                    return;
                }

                if (stat.Value <0 )
                {
                    _statsController.ProcessModifier(new StatModifier(
                        new Stat(StatType.Health, 0), StatModificatorType.Setter, -1, Time.time));
                    if (OnDead != null) OnDead(this, EventArgs.Empty);
                }
            }
        }
    }
}