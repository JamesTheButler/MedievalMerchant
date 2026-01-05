using System;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Elements;
using Common.UI.Tooltips;
using Features.Goods.Config;
using Features.Trade;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Common.UI.Popups
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
        private TitleDescriptionTooltipHandler availabilityTooltip;

        private readonly Lazy<AvailabilityResources> _availabilityResources =
            new(() => ResourceManager.Instance.AvailabilityResources);

        private readonly Lazy<GoodsResources> _goodsConfig = new(() => ResourceManager.Instance.GoodsResources);

        private Good _good;
        private Availability? _availability;
        private TradeType? _hoveredTradeType;

        private void Start()
        {
            buyButtonTooltip = buyButton.gameObject.GetComponent<SimpleTooltipHandler>();
            sellButtonTooltip = sellButton.gameObject.GetComponent<SimpleTooltipHandler>();

            buyButton.onClick.AddListener(() => TradeInitiated(TradeType.Buy));
            sellButton.onClick.AddListener(() => TradeInitiated(TradeType.Sell));
        }

        public void SetTradeType(TradeType? tradeType)
        {
            _hoveredTradeType = tradeType;
            buyButton.gameObject.SetActive(tradeType is null or TradeType.Buy);
            sellButton.gameObject.SetActive(tradeType is null or TradeType.Sell);
        }

        private void TradeInitiated(TradeType tradeType)
        {
            Hide();
            tradeInitiated?.Invoke(_good, tradeType);
        }

        public void SetGood(Good good)
        {
            _good = good;
            goodNameText.text = _goodsConfig.Value.ResourceData[good].GoodName;
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

        public void SetAvailability(Availability availability)
        {
            if (_availability == availability)
                return;

            _availability = availability;

            var configData = _availabilityResources.Value.Resources[availability];
            availabilityTooltip.SetData(($"Availability: {configData.DisplayString}", configData.Description));
        }
    }
}