using Common.Infrastructure;
using Common.Types;
using Common.UI.Elements;
using Common.UI.Utility;
using Features.Inventory;
using Features.Player.Logic;
using Features.Towns;
using Features.Trade;
using Features.Trade.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UI.Popups
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
        private Selection _selection;

        public void Initialize(GoodCell inventoryCell, TradeType tradeType)
        {
            Reset();

            _selection = GameplayContext.Instance.Selection;
            _selection.SelectedTown.Observe(OnSelectedTownChanged, false);

            if (_selection.SelectedTown.Value is null)
                return;

            if (inventoryCell.Good == null)
                return;

            _good = inventoryCell.Good.Value;

            _player = GameplayContext.Instance.Model.Player;
            _town = _selection.SelectedTown;

            _playerInventory = _player.Inventory;
            _townInventory = _town.Inventory;

            _tradeValidator = new TradeValidator(_player, _town);
            _availabilityCalculator = new AvailabilityCalculator(_town);

            var cellTransform = (RectTransform)inventoryCell.transform;
            var cellCenter = cellTransform.GetCenter();

            buySellPopup.Open();
            buySellPopup.transform.position = cellCenter + Vector3.up * yOffset;
            buySellPopup.SetGood(_good);
            buySellPopup.SetTradeType(tradeType);

            // can buy and sell?
            OnPlayerGoodUpdated(_good, _playerInventory.Get(_good));
            OnTownGoodUpdated(_good, _townInventory.Get(_good));
            _player.Location.CurrentTown.Observe(OnPlayerTownChanged);
            _playerInventory.GoodUpdated += OnPlayerGoodUpdated;
            _townInventory.GoodUpdated += OnTownGoodUpdated;
            _player.CaravanManager.SlotCount.Observe(OnTotalSlotCountChanged, false);
        }

        public void Reset()
        {
            buySellPopup.Close();

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
                _player.Location.CurrentTown.StopObserving(OnPlayerTownChanged);
                _player.CaravanManager.SlotCount.StopObserving(OnTotalSlotCountChanged);
            }
        }

        private void OnSelectedTownChanged(Town town)
        {
            Reset();
        }

        private void OnPlayerTownChanged(Town town)
        {
            ValidateButtons();
        }

        private void OnTownGoodUpdated(Good good, int amount)
        {
            if (_good != good)
                return;

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