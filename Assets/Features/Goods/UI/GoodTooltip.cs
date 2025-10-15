using Common;
using Common.Config;
using Common.Types;
using Common.UI;
using Features.Goods.Config;
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

        private GoodsConfig _goodsConfig;
        private TierIconConfig _tierIcons;
        private RegionIconConfig _regionIcons;

        private void Awake()
        {
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            _tierIcons = ConfigurationManager.Instance.TierIconConfig;
            _regionIcons = ConfigurationManager.Instance.RegionIconConfig;
        }

        public override void SetData(Good good)
        {
            var goodData = _goodsConfig.ConfigData[good];

            var tier = goodData.Tier;
            var price = _goodsConfig.BasePriceData[tier];
            var tierIcon = _tierIcons.Icons[tier];
            var regionIcon = _regionIcons.GetIcon(goodData.Regions);

            nameText.text = goodData.GoodName;
            priceText.text = $"{price:0.##}";
            tierImage.sprite = tierIcon;
            regionImage.sprite = regionIcon;
        }
    }
}