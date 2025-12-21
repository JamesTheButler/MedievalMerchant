using System.Collections.Generic;
using Common.Config;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Tooltips;
using Features.Goods.Config;
using Features.Player.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Goods.UI
{
    public sealed class PlayerGoodTooltip : TooltipBase<Good>
    {
        [SerializeField, Required]
        private TMP_Text nameText, priceText, averagePurchasePriceText;

        [SerializeField, Required]
        private Image tierImage, regionImage;

        private TradeTracker _tradeTracker;
        private GoodsResources _goodsResources;
        private GoodsConfig _goodsConfig;
        private TierResources _tierIcons;
        private RegionResources _region;

        protected override void Awake()
        {
            base.Awake();
            _tradeTracker = GameplayContext.Instance.Model.Player.TradeTracker;
            _goodsConfig = ConfigurationManager.Configurations.GoodsConfig;
            _goodsResources = ResourceManager.Instance.GoodsResources;
            _tierIcons = ResourceManager.Instance.TierResources;
            _region = ResourceManager.Instance.RegionResources;
        }

        protected override void UpdateUI(Good data)
        {
            var goodData = _goodsResources.ConfigData[data];
            var tier = goodData.Tier;
            var price = _goodsConfig.BasePriceData[tier];
            var tierIcon = _tierIcons.Icons[tier];
            // TODO - HACK: should it take First() instead of random?
            var regionIcon = _region.Data[goodData.Regions.GetRandom()];

            nameText.text = goodData.GoodName;
            priceText.text = $"{price:0.##}";
            var purchasedAverage = _tradeTracker.TrackedGoods.GetValueOrDefault(data)?.AveragePrice ?? 0f;
            averagePurchasePriceText.text = $"{purchasedAverage:0.##}";


            tierImage.sprite = tierIcon;
            regionImage.sprite = regionIcon.Icon;
        }

        public override void Reset() { }
    }
}