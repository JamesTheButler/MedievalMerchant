namespace Common.Modifiable
{
    public abstract class FlatModifier : IModifier
    {
        public Observable<float> Value { get; }
        public string FormattedValue => $"{Value.Value.Sign()}{Value.Value:0.##}";
        public string Description { get; }

        protected FlatModifier(float value, string description)
        {
            Value = new Observable<float>(value);
            Description = description;
        }
    }
}