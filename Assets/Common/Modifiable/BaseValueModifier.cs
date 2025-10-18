namespace Common.Modifiable
{
    public abstract class BaseValueModifier : IModifier
    {
        public Observable<float> Value { get; }
        public Observable<string> FormattedValue { get; } = new();
        public Observable<string> Description { get; }

        protected BaseValueModifier(float value, string description)
        {
            Value = new Observable<float>(value);
            Description = new Observable<string>(description);
            Value.Observe(UpdateFormattedValue);
        }

        private void UpdateFormattedValue(float value)
        {
            FormattedValue.Value = $"{value:0.##}";
        }
    }
}