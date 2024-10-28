using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Script
{
    [System.Serializable]
    public class Stat 
    {
        [SerializeField] private double baseValve;

        public List<double> modifiers;

        public double GetValue()
        {
            return baseValve + modifiers.Sum();
        }

        public void AddModifier(double modifier)
        {
            modifiers.Add(modifier);
        }

        public void RemoveModifier(int modifier)
        {
            modifiers.RemoveAt(modifier);
        }
    }
}