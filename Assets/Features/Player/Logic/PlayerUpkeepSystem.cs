using Common.Infrastructure;
using Features.Player.Caravan.Logic;
using Features.Player.Retinue.Logic;
using Features.Towns;

namespace Features.Player.Logic
{
    public sealed class PlayerUpkeepSystem : ISystem
    {
        private CaravanManager _caravanManager;
        private RetinueManager _retinueManager;
        private PlayerModel _playerModel;
        private PlayerLocation _playerLocation;
        private UpkeepFundsChangeModifier _caravanUpkeepFundsModifier, _retinueUpkeepFundsModifier;

        public void Initialize()
        {
            _playerModel = GameplayContext.Instance.Model.Player;
            _playerLocation = _playerModel.Location;
            _playerLocation.TownEntered += OnTownEntered;
            _playerLocation.TownExited += OnTownExited;

            _caravanManager = GameplayContext.Instance.Model.Player.CaravanManager;
            _retinueManager = GameplayContext.Instance.Model.Player.RetinueManager;

            _caravanUpkeepFundsModifier = new UpkeepFundsChangeModifier(_caravanManager.Upkeep, "Caravan");
            _retinueUpkeepFundsModifier = new UpkeepFundsChangeModifier(_retinueManager.Upkeep, "Retinue");
            _playerModel.FundsChange.AddModifier(_retinueUpkeepFundsModifier);
        }

        private void OnTownExited(Town town)
        {
            _playerModel.FundsChange.AddModifier(_caravanUpkeepFundsModifier);
        }

        private void OnTownEntered(Town town)
        {
            _playerModel.FundsChange.RemoveModifier(_caravanUpkeepFundsModifier);
        }

        public void CleanUp()
        {
            _playerLocation.TownEntered -= OnTownEntered;
            _playerLocation.TownExited -= OnTownExited;
        }
    }
}