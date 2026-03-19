using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Features.Localization.Data;
using Features.Player.Caravan.Logic;
using Features.Player.Retinue.Logic;
using Features.Towns;

namespace Features.Player.Logic
{
    public sealed class PlayerUpkeepSystem : ISystem
    {
        private CaravanManager _caravanManager;
        private RetinueModel _retinueModel;
        private PlayerModel _playerModel;
        private PlayerLocation _playerLocation;
        private PlayerLocalizationResources _loc;
        
        private UpkeepFundsChangeModifier _caravanUpkeepFundsModifier, _retinueUpkeepFundsModifier;

        public void Initialize()
        {
            _loc = ResourceManager.Instance.LocalizationResources.Player;
            _playerModel = GameplayContext.Instance.Model.Player;
            _playerLocation = _playerModel.Location;
            _playerLocation.CurrentTown.Observe(OnTownEntered);

            _caravanManager = GameplayContext.Instance.Model.Player.CaravanManager;
            _retinueModel = GameplayContext.Instance.Model.Player.RetinueModel;

            _caravanUpkeepFundsModifier = new UpkeepFundsChangeModifier(_caravanManager.Upkeep, _loc.CaravanUpkeep);
            _retinueUpkeepFundsModifier = new UpkeepFundsChangeModifier(_retinueModel.Upkeep, _loc.RetinueUpkeep);
            _playerModel.FundsChange.AddModifier(_retinueUpkeepFundsModifier);
        }

        private void OnTownEntered(Town town)
        {
            if (town == null)
            {
                _playerModel.FundsChange.AddModifier(_caravanUpkeepFundsModifier);
            }
            else
            {
                _playerModel.FundsChange.RemoveModifier(_caravanUpkeepFundsModifier);
            }
        }

        public void CleanUp()
        {
            _playerLocation.CurrentTown.StopObserving(OnTownEntered);
        }
    }
}