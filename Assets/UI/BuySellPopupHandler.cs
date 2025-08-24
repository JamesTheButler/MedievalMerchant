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
            buySellPopup.Hide();
        }

        public void Initialize(InventoryCell inventoryCell)
        {
            _good = inventoryCell.Good;

            _townInventory = Selection.Instance.SelectedTown.Inventory;
            _playerInventory = Model.Instance.Player.Inventory;

            // TODO: fix this
            //((RectTransform)buySellPopup.transform).anchoredPosition = inventoryCell.transform.localPosition;

            buySellPopup.SetGood(_good);

            // player inventory is checked for buying of goods
            var canSell = _playerInventory.Goods.ContainsKey(_good);
            buySellPopup.CanSell(canSell);
            _playerInventory.GoodUpdated += TryRefreshCanSell;

            // player inventory is checked for SALE of goods
            var canBuy = _townInventory.Goods.ContainsKey(_good);
            buySellPopup.CanBuy(canBuy);
            _townInventory.GoodUpdated += TryRefreshCanBuy;

            buySellPopup.Show();
        }

        public void Reset()
        {
            buySellPopup.Hide();

            _townInventory.GoodUpdated -= TryRefreshCanSell;
            _playerInventory.GoodUpdated -= TryRefreshCanBuy;

            _townInventory = null;
            _playerInventory = null;
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