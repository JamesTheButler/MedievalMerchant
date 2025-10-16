namespace Common.Modifiable
{
    public abstract class BaseValueModifier : IModifier
    {
        public float Value { get; }
        public string FormattedValue { get; }
        public string Description { get; }

        protected BaseValueModifier(float value, string description)
        {
            Value = value;
            FormattedValue = $"{Value:0.##}";
            Description = description;
        }
    }
}