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
    public sealed class SimpleGoodTooltip : TooltipBase<Good>
    {
        [SerializeField, Required]
        private TMP_Text nameText, priceText;

        [SerializeField, Required]
        private Image goodImage, tierImage, regionImage;

        private GoodResources _goodResources;
        private GoodConfig _goodConfig;
        private TierResources _tierIcons;
        private RegionResources _region;

        private Good _good;

        protected override void Awake()
        {
            base.Awake();
            _goodConfig = ConfigurationManager.Configurations.GoodConfig;
            _goodResources = ResourceManager.Instance.GoodResources;
            _tierIcons = ResourceManager.Instance.TierResources;
            _region = ResourceManager.Instance.RegionResources;
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
        }

        public override void Reset() { }
    }
}