using Common;
using Features.Player.Caravan.Config;

namespace Features.Player.Caravan.Logic
{
    public sealed class CaravanUpgrader
    {
        private readonly CaravanConfig _caravanConfig;
        private readonly PlayerModel _player;
        private readonly CaravanManager _caravanManager;

        public CaravanUpgrader()
        {
            _player = Model.Instance.Player;
            _caravanManager = _player.CaravanManager;
            _caravanConfig = ConfigurationManager.Instance.CaravanConfig;
        }

        public void RequestUpgrade(int cartId)
        {
            var cart = _caravanManager.Carts[cartId];
            var currentLevel = cart.Level.Value;

            if (currentLevel >= CaravanConfig.MaxLevel)
                return;

            var nextLevel = currentLevel + 1;
            var upgradeCost = _caravanConfig.GetUpgradeData(nextLevel).UpgradeCost;
            if (_player.Inventory.Funds < upgradeCost)
                return;

            _player.Inventory.RemoveFunds(upgradeCost);
            _caravanManager.UpgradeCart(cartId);
        }
    }
}