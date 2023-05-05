using StatsSystem.Enum;

namespace StatsSystem
{
    public interface IStatValueGiver
    {
        float GetStatsValue(StatType statType);
    }
}