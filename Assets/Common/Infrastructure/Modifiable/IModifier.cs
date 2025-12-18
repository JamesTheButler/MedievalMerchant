using Common.Infrastructure.Observation;

namespace Common.Infrastructure.Modifiable
{
    public interface IModifier
    {
        public Observable<float> Value { get; }
        public Observable<string> FormattedValue { get; }
        public Observable<string> Description { get; }
    }
}