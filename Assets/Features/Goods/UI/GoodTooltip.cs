using Common.Config;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Tooltips;
using Features.Goods.Config;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Goods.UI
{
    public class GoodTooltip : TooltipBase<Good>
    {
        [SerializeField, Required]
        protected TMP_Text nameText, priceText, currentPriceText;

        [SerializeField, Required]
        protected Image tierImage, regionImage;

        private GoodsResources _goodsResources;
        private GoodsConfig _goodsConfig;
        private TierResources _tierIcons;
        private RegionResources _region;

        protected override void Awake()
        {
            base.Awake();
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
            tierImage.sprite = tierIcon;
            regionImage.sprite = regionIcon.Icon;

            currentPriceText.gameObject.SetActive(false);
        }

        public override void Reset() { }
    }
}