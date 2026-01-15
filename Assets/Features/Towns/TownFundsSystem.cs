using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Features.Ticking.Logic;

namespace Features.Towns
{
    public sealed class TownFundsSystem : ISystem
    {
        private readonly Town _town;

        private TickingService _tickingService;
        private FloatBasedTicker _fundsChangeTicker;

        public TownFundsSystem(Town town)
        {
            _town = town;
        }

        public void Initialize()
        {
            _tickingService = GameplayContext.Instance.Services.TickingService;

            _fundsChangeTicker = new FloatBasedTicker(OnFundsChangeTick, _town.FundsChange);
            _town.FundsChange.Observe(OnFundsRateChanged);
            _tickingService.RegisterTicker(_fundsChangeTicker);
        }

        public void CleanUp()
        {
            _town.FundsChange.StopObserving(OnFundsRateChanged);
            _tickingService.UnregisterTicker(_fundsChangeTicker);
        }

        private void OnFundsRateChanged(float fundsChangeRate)
        {
            _fundsChangeTicker.ValueRatePerDay = fundsChangeRate;
        }

        private void OnFundsChangeTick(float fundsChange)
        {
            _town.Inventory.AddFunds(fundsChange);
        }
    }
}