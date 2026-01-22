using Common.Infrastructure.Observation;

namespace Features.Map
{
    public sealed class MapModeModel
    {
        public Observable<MapMode> MapMode { get; } = new();
    }
}