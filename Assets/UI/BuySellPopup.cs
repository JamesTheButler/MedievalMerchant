using System;
using Data;
using Data.Configuration;
using Data.Setup;
using Data.Trade;
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

        [SerializeField]
        private Image marketStateIcon;

        [SerializeField]
        private TMP_Text marketStateText;

        private readonly Lazy<MarketStateConfig> _marketStateConfig =
            new(() => ConfigurationManager.Instance.MarketStateConfig);

        private readonly Lazy<GoodsConfig> _goodsConfig = new(() => ConfigurationManager.Instance.GoodsConfig);

        private Good _good;
        private MarketState? _marketState;

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
            goodNameText.text = _goodsConfig.Value.ConfigData[good].GoodName;
        }

        public void CanBuy(bool canBuy)
        {
            buyButton.interactable = canBuy;
        }

        public void CanSell(bool canSell)
        {
            sellButton.interactable = canSell;
        }

        public void SetMarketState(MarketState marketState)
        {
            if (_marketState == marketState) return;

            var configData = _marketStateConfig.Value.ConfigData[marketState];
            marketStateIcon.sprite = configData.Icon;
            marketStateText.text = configData.DisplayString;

            _marketState = marketState;
        }
    }
}