namespace Common.Modifiable
{
    public abstract class BaseValueModifier : IModifier
    {
        public float Value { get; }
        public string Description { get; }

        protected BaseValueModifier(float value, string description)
        {
            Value = value;
            Description = description;
        }

        public string ToDisplayString()
        {
            return $"{Value.Sign()}{Value:0.##} .. {Description}";
        }
    }
}