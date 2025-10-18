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
        public Observable<string> FormattedValue { get; } = new();
        public Observable<string> Description { get; }

        protected BasePercentageModifier(float value, string description)
        {
            Value = new Observable<float>(value);
            Description = new Observable<string>(description);
            Value.Observe(UpdateFormattedValue);
        }

        private void UpdateFormattedValue(float value)
        {
            FormattedValue.Value = $"{value.Sign()}{value * 100:0.##}%";
        }
    }
}