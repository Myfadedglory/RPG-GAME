using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    [System.Serializable]
    public class Stat 
    {
        [SerializeField] private double baseValve;

        public List<Modifier> Modifiers = new List<Modifier>();

        private bool dirty = true;

        private void MarkDirty()
        {
            dirty = true;
        }

        public double GetValue()
        {
            return GetFinalValve();
        }

        public void SetDefaultValue(double value)
        {
            baseValve = value;
        }

        public void AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
            MarkDirty();
            CalculateFinalValve();
        }

        public void RemoveModifier(int modifier)
        {
            Modifiers.RemoveAt(modifier);
            MarkDirty();
            CalculateFinalValve();
        }

        public double GetFinalValve()
        {
            if (dirty)
            {
                dirty = false;
            }
            return CalculateFinalValve();
        }

        private double CalculateFinalValve()
        {
            var finalValve = baseValve;
            foreach (var modifier in Modifiers)
            {
                if (modifier.GetOperation() == Modifier.Operation.Addition)
                {
                    finalValve += modifier.GetValue();
                }else if (modifier.GetOperation() == Modifier.Operation.Multiplication)
                {
                    finalValve *= modifier.GetValue();
                }
            }
            return finalValve;
        }
    }
}