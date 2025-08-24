using Data;
using UnityEngine;

namespace UI
{
    public class BuySellPopupHandler : MonoBehaviour
    {
        [SerializeField]
        private BuySellPopup buySellPopup;

        private Good _good;
        private Inventory _townInventory;
        private Inventory _playerInventory;

        private void Start()
        {
            Reset();

            Selection.Instance.TownSelected += _ => { Reset(); };
        }

        public void Initialize(InventoryCell inventoryCell)
        {
            Reset();

            _good = inventoryCell.Good;

            _townInventory = Selection.Instance.SelectedTown.Inventory;
            _playerInventory = Model.Instance.Player.Inventory;

            // TODO: fix popup position
            //((RectTransform)buySellPopup.transform).anchoredPosition = inventoryCell.transform.localPosition;

            buySellPopup.SetGood(_good);

            // can buy and sell?
            TryRefreshCanSell(_good, _playerInventory.Get(_good));
            TryRefreshCanBuy(_good, _townInventory.Get(_good));
            _playerInventory.GoodUpdated += TryRefreshCanSell;
            _townInventory.GoodUpdated += TryRefreshCanBuy;

            buySellPopup.Show();
        }

        public void Reset()
        {
            buySellPopup.Hide();

            if (_playerInventory != null)
            {
                _playerInventory.GoodUpdated -= TryRefreshCanSell;
                _playerInventory = null;
            }

            if (_townInventory != null)
            {
                _townInventory.GoodUpdated -= TryRefreshCanBuy;
                _townInventory = null;
            }
        }

        private void TryRefreshCanBuy(Good good, int amount)
        {
            if (_good == good)
            {
                buySellPopup.CanBuy(amount > 0);
            }
        }

        private void TryRefreshCanSell(Good good, int amount)
        {
            if (_good == good)
            {
                buySellPopup.CanSell(amount > 0);
            }
        }
    }
}