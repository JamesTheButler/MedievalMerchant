using System;
using Common.Infrastructure;
using Common.UI.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.Trade.Haggling.UI
{
    public sealed class HaggleToggle : MonoBehaviour, IPointerClickHandler
    {
        public event Action<HaggleLevel> Selected;

        [field: SerializeField]
        public HaggleLevel HaggleLevel { get; private set; }

        [SerializeField, Required]
        private TMP_Text titleText, coinText, reputationText;

        [SerializeField, Required]
        private GameObject selectionFrame;

        public void SetUp(TradeType tradeType)
        {
            var levelName = ResourceManager.Instance.HaggleResources.GetName(HaggleLevel);
            titleText.text = levelName;

            var configs = ConfigurationManager.Configurations.HaggleConfig.Configs[HaggleLevel];
            var priceChangeOnBuy = configs.PriceDifferenceOnBuy;
            // config price change is based on the buying price. for selling, we need to invert it
            var displayPriceChange = tradeType == TradeType.Buy ? priceChangeOnBuy : priceChangeOnBuy * -1;
            var reputation = configs.ReputationPer100Goods;

            var priceChangeStyle = HaggleLevel switch
            {
                HaggleLevel.VeryKind or HaggleLevel.Kind => Style.Bad,
                HaggleLevel.Tough or HaggleLevel.VeryTough => Style.Good,
                _ => Style.Default
            };

            var loc = ResourceManager.Instance.LocalizationResources.TradeStrings;
            coinText.text = loc.HaggleCoinEffect(displayPriceChange).WithStyle(priceChangeStyle);
            reputationText.text = loc.HaggleRepEffect(reputation).WithStyle(reputation.GetNumberStyle());
        }

        public void Toggle(bool isToggled)
        {
            selectionFrame.SetActive(isToggled);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Selected?.Invoke(HaggleLevel);
        }
    }
}