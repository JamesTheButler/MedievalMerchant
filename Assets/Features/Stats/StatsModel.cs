using Common.Infrastructure.Observation;

namespace Features.Stats
{
    public sealed class StatsModel
    {
        public Observable<int> TradesAborted { get; private set; } = new();
        public Observable<int> TradesCompleted { get; private set; } = new();
        public Observable<float> TradeVolumeTraded { get; private set; } = new();
        public Observable<float> TotalValueBought { get; private set; } = new();
        public Observable<float> TotalValueSold { get; private set; } = new();
    }
}