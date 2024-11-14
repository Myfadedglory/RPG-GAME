using UnityEngine;

namespace Script.Stats
{
    [System.Serializable]
    public class MagicStat : Stat
    {
        [SerializeField] private float statusDuration;
        
        public ValueStat magicResistance;

        public float GetMagicStatusDuration()
        {
            return statusDuration;
        }
    }
}