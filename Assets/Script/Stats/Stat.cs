using System.Collections.Generic;
using UnityEngine;

namespace Script.Stats
{
    [System.Serializable]
    public class Stat 
    {
        [SerializeField] protected double baseValve;

        public List<Modifier> Modifiers = new ();

        private bool dirty = true;

        protected virtual void MarkDirty()
        {
            dirty = true;
        }

        public virtual double GetValue()
        {
            return GetFinalValve();
        }

        public virtual void SetDefaultValue(double value)
        {
            baseValve = value;
        }

        public virtual void AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
            
            MarkDirty();
            
            CalculateFinalValve();
        }

        public virtual void RemoveModifier(Modifier modifier)
        {
            for (var i = 0; i < Modifiers.Count; i++)
            {
                if(Modifiers[i].GetGuid() == modifier.GetGuid())
                    Modifiers.RemoveAt(i);
            }
            
            MarkDirty();
            
            CalculateFinalValve();
        }

        public virtual double GetFinalValve()
        {
            if (dirty)
            {
                dirty = false;
            }
            
            return CalculateFinalValve();
        }

        protected virtual double CalculateFinalValve()
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