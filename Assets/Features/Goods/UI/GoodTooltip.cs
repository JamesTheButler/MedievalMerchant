using Common.Config;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Tooltips;
using Features.Goods.Config;
using Features.Towns;
using Features.Trade;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Features.Goods.UI
{
    public class GoodTooltip : TooltipBase<Good>
    {
        [SerializeField, Required]
        protected TMP_Text currentPriceLabel;

        [SerializeField, Required]
        private TMP_Text nameText, priceText, currentPriceText;

        [SerializeField, Required]
        protected GameObject currentPriceLine;

        [SerializeField, Required]
        private Image goodImage, tierImage, regionImage;

        [SerializeField]
        private LocalizedString sellPriceString, buyPriceString;

        private GoodResources _goodResources;
        private GoodConfig _goodConfig;
        private TierResources _tierIcons;
        private RegionResources _region;
        private Selection _selection;

        private Good _good;
        private Town _town;

        protected override void Awake()
        {
            base.Awake();
            _goodConfig = ConfigurationManager.Configurations.GoodConfig;
            _goodResources = ResourceManager.Instance.GoodResources;
            _tierIcons = ResourceManager.Instance.TierResources;
            _region = ResourceManager.Instance.RegionResources;
            _selection = GameplayContext.Instance.Selection;
        }

        protected override void UpdateUI(Good data)
        {
            _good = data;

            var goodData = _goodResources.ResourceData[_good];
            var tier = goodData.Tier;
            var price = _goodConfig.BasePriceData[tier];
            var tierIcon = _tierIcons.Icons[tier];
            // TODO - HACK: should it take First() instead of random?
            var regionIcon = _region.Data[goodData.Regions.GetRandom()];

            nameText.text = goodData.GoodName;
            priceText.text = $"{price:0.00}";

            goodImage.sprite = goodData.Icon;
            tierImage.sprite = tierIcon;
            regionImage.sprite = regionIcon.Icon;

            _selection.SelectedTown.Observe(OnTownChanged);
        }

        private void OnTownChanged(Town newTown)
        {
            // unobserve the old price
            _town?.PriceManager.GetPrice(_good, TradeType.Buy)?.StopObserving(OnTownPriceChanged);
            _town?.PriceManager.GetPrice(_good, TradeType.Sell)?.StopObserving(OnTownPriceChanged);

            _town = newTown;
            if (newTown == null)
            {
                currentPriceLine.SetActive(false);
                currentPriceText.text = "-";
                return;
            }

            var buyPrice = newTown.PriceManager.GetPrice(_good, TradeType.Buy);
            var sellPrice = newTown.PriceManager.GetPrice(_good, TradeType.Sell);

            if (buyPrice != null)
            {
                currentPriceLine.SetActive(true);
                currentPriceLabel.text = sellPriceString.GetLocalizedString();
                buyPrice.Observe(OnTownPriceChanged);
            }
            else if (sellPrice != null)
            {
                currentPriceLine.SetActive(true);
                currentPriceLabel.text = buyPriceString.GetLocalizedString();
                sellPrice.Observe(OnTownPriceChanged);
            }
            else
            {
                currentPriceLine.SetActive(false);
                Debug.LogError($"There is neither a buy nor sell price available in {_town.Name} for {_good}.");
            }
        }

        private void OnTownPriceChanged(float price)
        {
            currentPriceText.text = $"{price:0.00}";
        }

        public override void Reset()
        {
            _selection.SelectedTown.StopObserving(OnTownChanged);
            _town?.PriceManager.GetPrice(_good, TradeType.Buy)?.StopObserving(OnTownPriceChanged);
            _town?.PriceManager.GetPrice(_good, TradeType.Sell)?.StopObserving(OnTownPriceChanged);
        }
    }
}