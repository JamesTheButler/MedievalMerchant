using Common.Types;
using Common.UI;
using Features.Inventory;
using Features.Player;
using Features.Towns;
using Features.Trade;
using Features.Trade.Logic;
using Infrastructure;
using NaughtyAttributes;
using UI.InventoryUI;
using UnityEngine;

namespace UI.Popups
{
    public sealed class BuySellPopupHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private BuySellPopup buySellPopup;

        [SerializeField]
        private float yOffset;

        private Good _good;
        private PlayerModel _player;
        private Town _town;
        private Inventory _townInventory;
        private Inventory _playerInventory;
        private AvailabilityCalculator _availabilityCalculator;
        private TradeValidator _tradeValidator;

        private void Start()
        {
            Reset();
            GameplayContext.Instance.Selection.TownSelected += _ => Reset();
        }

        public void Initialize(InventoryCellBase inventoryCell, TradeType tradeType)
        {
            Reset();

            var selection = GameplayContext.Instance.Selection;
            if (selection.SelectedTown is null) return;
            if (inventoryCell.Good == null) return;

            _good = inventoryCell.Good.Value;

            _player = GameplayContext.Instance.Model.Player;
            _town = selection.SelectedTown;

            _playerInventory = _player.Inventory;
            _townInventory = _town.Inventory;

            _tradeValidator = new TradeValidator(_player, _town);
            _availabilityCalculator = new AvailabilityCalculator(_town);

            var cellTransform = (RectTransform)inventoryCell.transform;
            var cellCenter = cellTransform.GetCenter();

            buySellPopup.Show();
            buySellPopup.transform.position = cellCenter + Vector3.up * yOffset;
            buySellPopup.SetGood(_good);
            buySellPopup.SetTradeType(tradeType);

            // can buy and sell?
            OnPlayerGoodUpdated(_good, _playerInventory.Get(_good));
            OnTownGoodUpdated(_good, _townInventory.Get(_good));
            _player.Location.TownEntered += OnTownChanged;
            _player.Location.TownExited += OnTownChanged;
            _playerInventory.GoodUpdated += OnPlayerGoodUpdated;
            _townInventory.GoodUpdated += OnTownGoodUpdated;
            _player.CaravanManager.SlotCount.Observe(OnTotalSlotCountChanged, false);
        }


        public void Reset()
        {
            buySellPopup.Hide();

            if (_playerInventory != null)
            {
                _playerInventory.GoodUpdated -= OnPlayerGoodUpdated;
                _playerInventory = null;
            }

            if (_townInventory != null)
            {
                _townInventory.GoodUpdated -= OnTownGoodUpdated;
                _townInventory = null;
            }

            if (_player != null)
            {
                _player.Location.TownEntered -= OnTownChanged;
                _player.Location.TownExited -= OnTownChanged;
                _player.CaravanManager.SlotCount.StopObserving(OnTotalSlotCountChanged);
            }
        }

        private void OnTownChanged(Town town)
        {
            ValidateButtons();
        }

        private void OnTownGoodUpdated(Good good, int amount)
        {
            if (_good != good)
                return;

            var availability = _availabilityCalculator.GetAvailability(good);
            buySellPopup.SetAvailability(availability);
            ValidateBuyButton();
        }

        private void OnTotalSlotCountChanged(int slotCount)
        {
            ValidateButtons();
        }

        private void OnPlayerGoodUpdated(Good good, int amount)
        {
            if (_good != good)
                return;

            ValidateSellButton();
        }

        private void ValidateButtons()
        {
            ValidateBuyButton();
            ValidateSellButton();
        }

        private void ValidateBuyButton()
        {
            var canBuy = _tradeValidator.Validate(TradeType.Buy, _good, 1);
            buySellPopup.CanBuy(canBuy);
        }

        private void ValidateSellButton()
        {
            var canSell = _tradeValidator.Validate(TradeType.Sell, _good, 1);
            buySellPopup.CanSell(canSell);
        }
    }
}