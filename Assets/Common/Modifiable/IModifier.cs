namespace Common.Modifiable
{
    public interface IModifier
    {
        public Observable<float> Value { get; }

        public string FormattedValue { get; }
        public string Description { get; }
    }
}