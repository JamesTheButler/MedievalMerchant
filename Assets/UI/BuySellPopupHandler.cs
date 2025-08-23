using Data;
using UnityEngine;

namespace UI
{
    public class BuySellPopupHandler : MonoBehaviour
    {
        [SerializeField]
        private BuySellPopup buySellPopup;

        private Good? _good;
        private Inventory _thisInventory;
        private Inventory _otherInventory;
        
        public void Initialize(Vector2 position, Good good, Inventory thisInventory, Inventory otherInventory)
        {
            _good = good;
            _thisInventory = thisInventory;
            _otherInventory = otherInventory;

            ((RectTransform)buySellPopup.transform).anchoredPosition = position;

            buySellPopup.SetGood(good);
            
            // can sell and buy?
            var canBuy = otherInventory.Goods.ContainsKey(good);
            var canSell = thisInventory.Goods.ContainsKey(good);
            buySellPopup.CanBuy(canBuy);
            buySellPopup.CanSell(canSell);
            _thisInventory.GoodUpdated += TryRefreshCanSell;
            _otherInventory.GoodUpdated += TryRefreshCanBuy;

            buySellPopup.gameObject.SetActive(true);
        }

        public void Reset()
        {
            buySellPopup.gameObject.SetActive(false);

            _thisInventory.GoodUpdated -= TryRefreshCanSell;
            _otherInventory.GoodUpdated -= TryRefreshCanBuy;

            _good = null;
            _thisInventory = null;
            _otherInventory = null;
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