using Features.Ticking;
using Infrastructure;

namespace Features.Towns.Development.Logic
{
    public sealed class TownDevelopmentSystem : ISystem
    {
        private readonly Town _town;

        private FloatBasedTicker _developmentTicker;
        private TickingService _tickingService;

        public TownDevelopmentSystem(Town town)
        {
            _town = town;
        }

        public void Initialize()
        {
            _tickingService = GameplayContext.Instance.Services.TickingService;

            _developmentTicker = new FloatBasedTicker(DevelopmentTick, _town.DevelopmentManager.DevelopmentTrend.Value);
            _town.DevelopmentManager.DevelopmentTrend.Observe(OnDevelopmentTrendChanged);
            _tickingService.RegisterTicker(_developmentTicker);
        }

        public void CleanUp()
        {
            _tickingService.UnregisterTicker(_developmentTicker);

            _town.DevelopmentManager.DevelopmentTrend.StopObserving(OnDevelopmentTrendChanged);
        }

        private void OnDevelopmentTrendChanged(float developmentTrend)
        {
            _developmentTicker.ValueRatePerDay = developmentTrend;
        }

        private void DevelopmentTick(float developmentChange)
        {
            _town.DevelopmentManager.AddDevelopmentChange(developmentChange);
        }
    }
}