using System.Linq;
using AYellowpaper.SerializedCollections;
using Data;
using Data.Configuration;
using Data.Setup;
using Data.Trade;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerInventoryUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text fundsText;

        [SerializeField, SerializedDictionary("Tier", "Section")]
        private SerializedDictionary<Tier, PlayerInventorySection> inventorySections;

        [SerializeField, SerializedDictionary("Upgrade", "Button")]
        private SerializedDictionary<PlayerUpgrade, UpgradeButton> upgradeButtons;

        private Player _player;
        private Inventory _playerInventory;
        private GoodsConfig _goodsConfig;
        private PlayerUpgradeConfigData _playerUpgradeConfig;

        public void Bind(Player player)
        {
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            _playerUpgradeConfig = ConfigurationManager.Instance.PlayerUpgradeConfigData;
            _player = player;
            _playerInventory = player.Inventory;

            SetUpUpgradeButtons();

            _player.UpgradeAdded += OnPlayerUpgradeAdded;

            foreach (var upgrade in _player.Upgrades)
            {
                OnPlayerUpgradeAdded(upgrade);
            }

            SetUpInventory();
        }

        public void Unbind()
        {
            _playerInventory.FundsUpdated -= OnFundsChanged;
            _playerInventory.GoodUpdated -= OnGoodUpdated;

            _player.UpgradeAdded -= OnPlayerUpgradeAdded;
        }

        private void OnPlayerUpgradeAdded(PlayerUpgrade upgrade)
        {
            upgradeButtons[upgrade].SetState(UpgradeButton.State.Hidden);
            var nextPossibleUpgrade = _playerUpgradeConfig.ProgressionData.GetNext(upgrade);
            if (nextPossibleUpgrade == PlayerUpgrade.None) return;

            upgradeButtons[nextPossibleUpgrade].SetState(UpgradeButton.State.Active);
        }

        private void SetUpInventory()
        {
            _playerInventory.GoodUpdated += OnGoodUpdated;
            _playerInventory.FundsUpdated += OnFundsChanged;

            OnFundsChanged(_playerInventory.Funds);
            foreach (var (good, amount) in _playerInventory.Goods)
            {
                OnGoodUpdated(good, amount);
            }
        }

        private void SetUpUpgradeButtons()
        {
            foreach (var (upgrade, button) in upgradeButtons)
            {
                var price = _playerUpgradeConfig.UpgradeCosts[upgrade];
                button.SetCost(price);
                button.SetState(UpgradeButton.State.Disabled);
                button.OnClick.AddListener(() => UpgradePlayer(upgrade, price));

                button.Validate(_playerInventory.Funds);
            }

            var progressions = _playerUpgradeConfig.ProgressionData.UpgradeProgressions;
            foreach (var progression in progressions)
            {
                var upgrade = progression.First();
                upgradeButtons[upgrade].SetState(UpgradeButton.State.Active);
            }
        }

        private void OnGoodUpdated(Good good, int amount)
        {
            var tier = _goodsConfig.ConfigData[good].Tier;
            var section = inventorySections[tier];
            section.UpdateGood(good, amount);
        }

        private void OnFundsChanged(int funds)
        {
            fundsText.text = funds.ToString();

            foreach (var upgradeButton in upgradeButtons.Values)
            {
                upgradeButton.Validate(funds);
            }
        }

        private void UpgradePlayer(PlayerUpgrade upgrade, int price)
        {
            if (!_playerInventory.HasFunds(price)) return;

            _playerInventory.RemoveFunds(price);
            _player.AddUpgrade(upgrade);
        }
    }
}