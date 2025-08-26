using Data;
using Data.Setup;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public sealed class BuySellPopup : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<Good, TradeType> tradeInitiated;

        [SerializeField]
        private TMP_Text goodNameText;

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Button sellButton;

        private Good _good;

        private void Start()
        {
            buyButton.onClick.AddListener(() => TradeInitiated(TradeType.Buy));
            sellButton.onClick.AddListener(() => TradeInitiated(TradeType.Sell));
        }

        private void TradeInitiated(TradeType tradeType)
        {
            Hide();
            tradeInitiated?.Invoke(_good, tradeType);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetGood(Good good)
        {
            _good = good;
            goodNameText.text = SetupManager.Instance.GoodInfoManager.GoodInfos[good].GoodName;
        }

        public void CanBuy(bool canBuy)
        {
            buyButton.interactable = canBuy;
        }

        public void CanSell(bool canSell)
        {
            sellButton.interactable = canSell;
        }
    }
}