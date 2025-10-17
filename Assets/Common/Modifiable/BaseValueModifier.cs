namespace Common.Modifiable
{
    public abstract class BaseValueModifier : IModifier
    {
        public Observable<float> Value { get; }
        public string FormattedValue => $"{Value.Value:0.##}";
        public string Description { get; }

        protected BaseValueModifier(float value, string description)
        {
            Value = new Observable<float>(value);
            Description = description;
        }
    }
}