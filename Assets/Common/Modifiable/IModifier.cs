namespace Common.Modifiable
{
    public interface IModifier
    {
        public Observable<float> Value { get; }
        public Observable<string> FormattedValue { get; }
        public Observable<string> Description { get; }
    }
}