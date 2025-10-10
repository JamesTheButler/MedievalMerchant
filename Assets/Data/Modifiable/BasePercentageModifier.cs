namespace Data.Modifiable
{
    /// <summary>
    /// Percentage of the current base value that is added.
    /// 0 .. 0%
    /// 1 .. 100%
    /// </summary>
    public abstract class BasePercentageModifier : IModifier
    {
        public float Value { get; }
        public string Description { get; }

        protected BasePercentageModifier(float value, string description)
        {
            Value = value;
            Description = description;
        }

        public string ToDisplayString()
        {
            return $"{Value.Sign()}{Value * 100:0.##}% .. {Description}";
        }
    }
}