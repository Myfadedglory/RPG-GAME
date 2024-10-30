namespace Script
{
    public class Modifier
    {
        private readonly string className;
        private readonly Operation operation;
        private readonly double value;

        public Modifier(string className, Operation operation, double value)
        {
            this.className = className;
            this.operation = operation;
            this.value = value;
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