using System;
using Data;
using Data.Configuration;
using Data.Setup;
using Data.Trade;
using NaughtyAttributes;
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

        [SerializeField, Required]
        private TMP_Text goodNameText;

        [SerializeField, Required]
        private Button buyButton;

        [SerializeField, Required]
        private Button sellButton;

        [SerializeField, Required]
        private Image marketStateIcon;

        [SerializeField, Required]
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

        public void CanBuy(TradeResult canBuy)
        {
            buyButton.interactable = canBuy.Success;
            // TODO: on error: add a tooltip as to why it cannot be bought
        }

        public void CanSell(TradeResult canSell)
        {
            sellButton.interactable = canSell.Success;
            // TODO: on error: add a tooltip as to why it cannot be sold
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