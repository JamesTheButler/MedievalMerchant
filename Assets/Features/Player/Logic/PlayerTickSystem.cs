using Common.Infrastructure;
using Features.Ticking;

namespace Features.Player.Logic
{
    public sealed class PlayerTickSystem : ISystem
    {
        private TickingService _tickingService;
        private FloatBasedTicker _fundChangeTicker;
        private PlayerModel _playerModel;

        public void Initialize()
        {
            _tickingService = GameplayContext.Instance.Services.TickingService;
            _playerModel = GameplayContext.Instance.Model.Player;
            _fundChangeTicker = new FloatBasedTicker(OnTick, _playerModel.FundsChange.Value);

            _tickingService.RegisterTicker(_fundChangeTicker);
            _playerModel.FundsChange.Observe(OnFundsChangeChanged);
        }

        private void OnFundsChangeChanged(float fundsChangeRate)
        {
            _fundChangeTicker.ValueRatePerDay = fundsChangeRate;
        }

        public void CleanUp()
        {
            _tickingService.UnregisterTicker(_fundChangeTicker);
            _playerModel.FundsChange.StopObserving(OnFundsChangeChanged);
        }

        private void OnTick(float fundsChange)
        {
            _playerModel.Inventory.AddFunds(fundsChange);
        }
    }
}