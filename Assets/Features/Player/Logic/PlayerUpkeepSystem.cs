using Common.Infrastructure;
using Features.Player.Caravan.Logic;
using Features.Towns;

namespace Features.Player
{
    public sealed class PlayerUpkeepSystem : ISystem
    {
        private CaravanManager _caravanManager;
        private PlayerModel _playerModel;
        private PlayerLocation _playerLocation;
        private UpkeepFundsChangeModifier _upkeepFundsModifier;

        public void Initialize()
        {
            _playerModel = GameplayContext.Instance.Model.Player;
            _playerLocation = _playerModel.Location;
            _playerLocation.TownEntered += OnTownEntered;
            _playerLocation.TownExited += OnTownExited;

            _caravanManager = GameplayContext.Instance.Model.Player.CaravanManager;

            _upkeepFundsModifier = new UpkeepFundsChangeModifier(_caravanManager.Upkeep);
        }

        private void OnTownExited(Town town)
        {
            _playerModel.FundsChange.AddModifier(_upkeepFundsModifier);
        }

        private void OnTownEntered(Town town)
        {
            _playerModel.FundsChange.RemoveModifier(_upkeepFundsModifier);
        }

        public void CleanUp()
        {
            _playerLocation.TownEntered -= OnTownEntered;
            _playerLocation.TownExited -= OnTownExited;
        }
    }
}