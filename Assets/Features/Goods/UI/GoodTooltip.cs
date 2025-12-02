using Common;
using Common.Config;
using Common.Types;
using Common.UI;
using Features.Goods.Config;
using Infrastructure;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Goods.UI
{
    public sealed class GoodTooltip : TooltipBase<Good>
    {
        [SerializeField, Required]
        private TMP_Text nameText, priceText;

        [SerializeField, Required]
        private Image tierImage, regionImage;

        private GoodsResources _goodsResources;
        private GoodsConfig _goodsConfig;
        private TierResources _tierIcons;
        private RegionResources _region;

        protected override void Awake()
        {
            base.Awake();
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
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
            tierImage.sprite = tierIcon;
            regionImage.sprite = regionIcon.Icon;
        }

        public override void Reset() { }
    }
}