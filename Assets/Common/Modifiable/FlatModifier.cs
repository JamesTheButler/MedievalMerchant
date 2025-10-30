namespace Common.Modifiable
{
    public abstract class FlatModifier : IModifier
    {
        public Observable<float> Value { get; }
        public Observable<string> FormattedValue { get; } = new();
        public Observable<string> Description { get; }

        protected FlatModifier(float value, string description) : this(new Observable<float>(value), description) { }

        protected FlatModifier(Observable<float> observable, string description)
        {
            Value = observable;
            Description = new Observable<string>(description);

            Value.Observe(UpdateFormattedValue);
        }

        private void UpdateFormattedValue(float value)
        {
            FormattedValue.Value = $"{value.Sign()}{value:0.##}";
        }
    }
}