using Data;
using Data.Towns;
using UnityEngine;

namespace UI
{
    public sealed class BuySellPopupHandler : MonoBehaviour
    {
        [SerializeField]
        private BuySellPopup buySellPopup;

        private Good _good;
        private Inventory _townInventory;
        private Inventory _playerInventory;
        private MarketStateManager _marketStateManager;

        private void Start()
        {
            Reset();

            Selection.Instance.TownSelected += _ => { Reset(); };
        }

        public void Initialize(InventoryCell inventoryCell)
        {
            Reset();

            if (Selection.Instance.SelectedTown is null) return;

            _good = inventoryCell.Good;

            _townInventory = Selection.Instance.SelectedTown.Inventory;
            _playerInventory = Model.Instance.Player.Inventory;

            _marketStateManager = new MarketStateManager(_townInventory);

            // TODO: fix popup position
            //((RectTransform)buySellPopup.transform).anchoredPosition = inventoryCell.transform.localPosition;

            buySellPopup.SetGood(_good);

            // can buy and sell?
            OnPlayerGoodUpdated(_good, _playerInventory.Get(_good));
            OnTownGoodUpdated(_good, _townInventory.Get(_good));
            _playerInventory.GoodUpdated += OnPlayerGoodUpdated;
            _townInventory.GoodUpdated += OnTownGoodUpdated;

            buySellPopup.Show();
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

            buySellPopup.CanBuy(amount > 0);
            var marketState = _marketStateManager.GetMarketState(good);
            buySellPopup.SetMarketState(marketState);
        }

        private void OnPlayerGoodUpdated(Good good, int amount)
        {
            if (_good == good)
            {
                buySellPopup.CanSell(amount > 0);
            }
        }
    }
}