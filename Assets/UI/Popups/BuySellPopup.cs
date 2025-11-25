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
        private Button buyButton, sellButton;

        [SerializeField, Required]
        private SimpleTooltipHandler buyButtonTooltip, sellButtonTooltip;

        [SerializeField, Required]
        private Hoverable buyButtonHoverable, sellButtonHoverable;

        [SerializeField, Required]
        private Image marketStateIcon;

        [SerializeField, Required]
        private SimpleTooltipHandler marketStateTooltip;

        private readonly Lazy<AvailabilityConfig> _marketStateConfig =
            new(() => ConfigurationManager.Instance.AvailabilityConfig);

        private readonly Lazy<GoodsConfig> _goodsConfig = new(() => ConfigurationManager.Instance.GoodsConfig);

        private Good _good;
        private Availability? _marketState;
        private TradeType? _hoveredTradeType;

        private void Start()
        {
            buyButtonTooltip = buyButton.gameObject.GetComponent<SimpleTooltipHandler>();
            sellButtonTooltip = sellButton.gameObject.GetComponent<SimpleTooltipHandler>();

            buyButton.onClick.AddListener(() => TradeInitiated(TradeType.Buy));
            
            sellButton.onClick.AddListener(() => TradeInitiated(TradeType.Sell));
            
            // TODO: maybe reimplement this. it's not very clear as is
            //buyButtonHoverable.Hovered += () => SetHoveredTradeType(TradeType.Buy);
            //buyButtonHoverable.Unhovered += () => SetHoveredTradeType(null);
            //sellButtonHoverable.Hovered += () => SetHoveredTradeType(TradeType.Sell);
            //sellButtonHoverable.Unhovered += () => SetHoveredTradeType(null);
        }

        private void SetHoveredTradeType(TradeType? type)
        {
            _hoveredTradeType = type;
            RefreshIcon();
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
            buyButtonTooltip.SetData(canBuy.Error);
        }

        public void CanSell(TradeResult canSell)
        {
            sellButton.interactable = canSell.Success;
            sellButtonTooltip.SetEnabled(!canSell.Success);
            sellButtonTooltip.SetData(canSell.Error);
        }

        public void SetMarketState(Availability availability)
        {
            if (_marketState == availability)
                return;

            _marketState = availability;
            RefreshIcon();

            var configData = _marketStateConfig.Value.ConfigData[availability];
            marketStateTooltip.SetData($"Availability: {configData.DisplayString}.\n{configData.Description}");
        }

        private void RefreshIcon()
        {
            var configData = _marketStateConfig.Value.ConfigData[_marketState!.Value];

            var icon = _hoveredTradeType switch
            {
                TradeType.Buy => configData.BuyIcon,
                TradeType.Sell => configData.SellIcon,
                _ => configData.DefaultIcon
            };

            marketStateIcon.sprite = icon;
        }
    }
}