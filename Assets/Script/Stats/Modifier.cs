using System;
using UnityEngine;

namespace Script.Stats
{
    [Serializable]
    public class Modifier
    {
        [SerializeField] private string className;
        [SerializeField] private StatType statType;
        [SerializeField] private Operation operation;
        [SerializeField] private double value;
        private readonly Guid guid;

        public Modifier(string className, StatType statType, Operation operation, double value)
        {
            this.className = className;
            this.statType = statType;
            this.operation = operation;
            this.value = value;
            guid = new Guid();
        }

        public string GetClassName()
        {
            return className;
        }

        public StatType GetStatType()
        {
            return statType;
        }

        public Operation GetOperation()
        {
            return operation;
        }

        public double GetValue()
        {
            return value;
        }
        
        public Guid GetGuid()
        {
            return guid;
        }
        
        public enum Operation
        {
            Addition,
            Multiplication
        }
    }
}