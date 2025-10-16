namespace Common.Modifiable
{
    public abstract class FlatModifier : IModifier
    {
        public float Value { get; }
        public string FormattedValue { get; }
        public string Description { get; }

        protected FlatModifier(float value, string description)
        {
            Value = value;
            FormattedValue = $"{Value.Sign()}{Value:0.##}";
            Description = description;
        }
    }
}