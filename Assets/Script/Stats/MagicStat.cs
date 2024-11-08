using UnityEngine;

namespace Script.Stats
{
    [System.Serializable]
    public class MagicStat : Stat
    {
        [SerializeField] private float statusDuration;
        
        public Stat magicResistance;

        public float GetMagicStatusDuration()
        {
            return statusDuration;
        }
    }
}