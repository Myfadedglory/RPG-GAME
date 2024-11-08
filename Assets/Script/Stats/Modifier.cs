using System;

namespace Script.Stats
{
    public class Modifier
    {
        private readonly string className;
        private readonly Operation operation;
        private readonly double value;
        private readonly Guid guid;

        public Modifier(string className, Operation operation, double value)
        {
            this.className = className;
            this.operation = operation;
            this.value = value;
            guid = new Guid();
        }

        public Guid GetGuid()
        {
            return guid;
        }

        public string GetClassName()
        {
            return className;
        }

        public Operation GetOperation()
        {
            return operation;
        }

        public double GetValue()
        {
            return value;
        }
        
        public enum Operation
        {
            Addition,
            Multiplication
        }
    }
}