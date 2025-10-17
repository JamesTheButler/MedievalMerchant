namespace Common.Modifiable
{
    /// <summary>
    /// Percentage of the current base value that is added.
    /// 0 .. 0%
    /// 1 .. 100%
    /// </summary>
    public abstract class BasePercentageModifier : IModifier
    {
        public Observable<float> Value { get; }
        public string FormattedValue => $"{Value.Value.Sign()}{Value.Value * 100:0.##}%";
        public string Description { get; }

        protected BasePercentageModifier(float value, string description)
        {
            Value = new Observable<float>(value);
            Description = description;
        }
    }
}