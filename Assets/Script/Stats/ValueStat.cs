using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Stats
{
    public enum ValueStatType
    {
        ValueAddition,
        ValueDecrease,
        PercentageAddition,
        PercentageDecrease,
    }
    
    [System.Serializable]
    public class ValueStat : Stat
    {
        [SerializeField] private ValueStatType statType;

        public double ApplyStat(double value, double multiple = 1)
        {
            return statType switch
            {
                ValueStatType.ValueAddition => value + multiple * GetValue(),
                ValueStatType.ValueDecrease => value - multiple * GetValue(),
                ValueStatType.PercentageAddition => value * (1 + multiple * GetValue()),
                ValueStatType.PercentageDecrease => value * (1 - multiple * GetValue()),
                _ => value
            };
        }
    }
}