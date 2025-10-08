using System.Linq;
using AYellowpaper.SerializedCollections;
using Data;
using Data.Configuration;
using Data.Player;
using Data.Trade;
using NaughtyAttributes;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.InventoryUI
{
    public class PlayerInventoryUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private UnityEvent<InventoryCell> inventoryCellClicked;

        [SerializeField, Required]
        private TMP_Text fundsText;

        [SerializeField, SerializedDictionary("Tier", "Section")]
        private SerializedDictionary<Tier, InventorySection> inventorySections;

        [SerializeField, SerializedDictionary("Upgrade", "Button")]
        private SerializedDictionary<PlayerUpgrade, UpgradeButton> upgradeButtons;

        private PlayerModel _player;
        private Inventory _playerInventory;
        private GoodsConfig _goodsConfig;
        private PlayerConfig _playerConfig;

        public void Bind(PlayerModel player)
        {
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            _playerConfig = ConfigurationManager.Instance.PlayerConfig;
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
            _playerInventory.Funds.StopObserving(OnFundsChanged);
            _playerInventory.GoodUpdated -= OnGoodUpdated;

            _player.UpgradeAdded -= OnPlayerUpgradeAdded;


            foreach (var section in inventorySections.Values)
            {
                section.CellClicked -= InvokeCellClicked;
                section.CleanUp();
            }
        }

        private void OnPlayerUpgradeAdded(PlayerUpgrade upgrade)
        {
            upgradeButtons[upgrade].SetState(UpgradeButton.State.Hidden);
            var nextPossibleUpgrade = _playerConfig.ProgressionData.GetNext(upgrade);
            if (nextPossibleUpgrade == PlayerUpgrade.None) return;

            upgradeButtons[nextPossibleUpgrade].SetState(UpgradeButton.State.Active);
        }

        private void SetUpInventory()
        {
            _playerInventory.Funds.Observe(OnFundsChanged);
            _playerInventory.GoodUpdated += OnGoodUpdated;

            foreach (var (good, amount) in _playerInventory.Goods)
            {
                OnGoodUpdated(good, amount);
            }
        }

        private void SetUpUpgradeButtons()
        {
            foreach (var section in inventorySections.Values)
            {
                section.Initialize();
                section.CellClicked += InvokeCellClicked;
            }

            foreach (var (upgrade, button) in upgradeButtons)
            {
                var upgradeData = _playerConfig.InventoryUpgrades[upgrade];
                button.SetCost(upgradeData.Price);
                button.SetState(UpgradeButton.State.Disabled);
                button.OnClick.AddListener(() => UpgradePlayer(upgrade, upgradeData.Price));

                button.Validate(_playerInventory.Funds);
            }

            var progressions = _playerConfig.ProgressionData.UpgradeProgressions;
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

        private void OnFundsChanged(float funds)
        {
            fundsText.text = funds.ToString("N0");

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

        private void InvokeCellClicked(InventoryCell cell)
        {
            inventoryCellClicked?.Invoke(cell);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PopupManager.Instance.HideActive();
        }
    }
}