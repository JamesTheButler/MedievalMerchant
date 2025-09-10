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
        private TooltipHandler buyButtonTooltip;

        [SerializeField, Required]
        private Button sellButton;

        [SerializeField, Required]
        private TooltipHandler sellButtonTooltip;

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
            buyButtonTooltip = buyButton.gameObject.GetComponent<TooltipHandler>();
            sellButtonTooltip = sellButton.gameObject.GetComponent<TooltipHandler>();

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
            buyButtonTooltip.SetEnabled(!canBuy.Success);
            buyButtonTooltip.SetTooltip(canBuy.Error);
        }

        public void CanSell(TradeResult canSell)
        {
            sellButton.interactable = canSell.Success;
            sellButtonTooltip.SetEnabled(!canSell.Success);
            sellButtonTooltip.SetTooltip(canSell.Error);
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