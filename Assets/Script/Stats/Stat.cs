using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Stats
{
    [System.Serializable]
    public class Stat 
    {
        [SerializeField] protected double baseValve;

        public List<Modifier> modifiers = new ();

        public Action StatValueChanged;

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
            modifiers.Add(modifier);
            
            StatValueChanged?.Invoke();
            
            MarkDirty();
            
            CalculateFinalValve();
        }

        public virtual void RemoveModifier(Modifier modifier)
        {
            for (var i = 0; i < modifiers.Count; i++)
            {
                if(modifiers[i].GetGuid() == modifier.GetGuid())
                    modifiers.RemoveAt(i);
            }
            
            StatValueChanged?.Invoke();
            
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
            
            foreach (var modifier in modifiers)
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