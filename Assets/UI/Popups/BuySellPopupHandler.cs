using System.Linq;
using Data;
using Data.Player;
using Data.Towns;
using Data.Trade;
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
            Selection.Instance.TownSelected += _ => { Reset(); };
        }

        public void Initialize(InventoryCellBase inventoryCell)
        {
            Reset();

            if (Selection.Instance.SelectedTown is null) return;
            if (inventoryCell.Good == null) return;

            _good = inventoryCell.Good.Value;

            _player = Model.Instance.Player;
            _town = Selection.Instance.SelectedTown;

            _playerInventory = _player.Inventory;
            _townInventory = _town.Inventory;

            _tradeValidator = new TradeValidator(_player, _town);
            _availabilityCalculator = new AvailabilityCalculator(_town);

            var cellTransform = (RectTransform)inventoryCell.transform;
            var arr = new Vector3[4];
            cellTransform.GetWorldCorners(arr);
            var cellCenter = arr.Aggregate(Vector3.zero, (curr, next) => curr + next / arr.Length);

            buySellPopup.Show();
            buySellPopup.transform.position = cellCenter + Vector3.up * yOffset;
            buySellPopup.SetGood(_good);

            // can buy and sell?
            OnPlayerGoodUpdated(_good, _playerInventory.Get(_good));
            OnTownGoodUpdated(_good, _townInventory.Get(_good));
            _player.Location.TownEntered += _ => ValidateButtons(); // TODO: need to unbind properly
            _player.Location.TownExited += _ => ValidateButtons(); // TODO: need to unbind properly
            _playerInventory.GoodUpdated += OnPlayerGoodUpdated;
            _townInventory.GoodUpdated += OnTownGoodUpdated;
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
        }

        private void OnTownGoodUpdated(Good good, int amount)
        {
            if (_good != good)
                return;

            var marketState = _availabilityCalculator.GetAvailability(good);
            buySellPopup.SetMarketState(marketState);
            ValidateBuyButton();
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