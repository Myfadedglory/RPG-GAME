using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat 
{
    [SerializeField] private double baseValve;

    public List<double> modifiers;

    public double GetValue()
    {
        double finalValue = baseValve;

        foreach (double modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
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