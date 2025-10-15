using System;
using Common;
using Common.Types;
using Common.UI;
using Features.Goods.Config;
using Features.Trade;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Popups
{
    public sealed class BuySellPopup : Popup
    {
        [SerializeField]
        private UnityEvent<Good, TradeType> tradeInitiated;

        [SerializeField, Required]
        private TMP_Text goodNameText;

        [SerializeField, Required]
        private Button buyButton;

        [SerializeField, Required]
        private SimpleTooltipHandler buyButtonTooltip;

        [SerializeField, Required]
        private Button sellButton;

        [SerializeField, Required]
        private SimpleTooltipHandler sellButtonTooltip;

        [SerializeField, Required]
        private Image marketStateIcon;

        [SerializeField, Required]
        private TMP_Text marketStateText;

        private readonly Lazy<AvailabilityConfig> _marketStateConfig =
            new(() => ConfigurationManager.Instance.AvailabilityConfig);

        private readonly Lazy<GoodsConfig> _goodsConfig = new(() => ConfigurationManager.Instance.GoodsConfig);

        private Good _good;
        private Availability? _marketState;

        private void Start()
        {
            buyButtonTooltip = buyButton.gameObject.GetComponent<SimpleTooltipHandler>();
            sellButtonTooltip = sellButton.gameObject.GetComponent<SimpleTooltipHandler>();

            buyButton.onClick.AddListener(() => TradeInitiated(TradeType.Buy));
            sellButton.onClick.AddListener(() => TradeInitiated(TradeType.Sell));
        }

        private void TradeInitiated(TradeType tradeType)
        {
            Hide();
            tradeInitiated?.Invoke(_good, tradeType);
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

        public void SetMarketState(Availability availability)
        {
            if (_marketState == availability) return;

            var configData = _marketStateConfig.Value.ConfigData[availability];
            marketStateIcon.sprite = configData.Icon;
            marketStateText.text = configData.DisplayString;

            _marketState = availability;
        }
    }
}