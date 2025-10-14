using Common;
using Common.Config;
using Common.Types;
using Features.Goods.Config;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods
{
    public sealed class GoodTooltipHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject tooltip;

        private GoodsConfig _goodsConfig;
        private TierIconConfig _tierIcons;
        private RegionIconConfig _regionIcons;
        private Canvas _canvas;

        private void Awake()
        {
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            _tierIcons = ConfigurationManager.Instance.TierIconConfig;
            _regionIcons = ConfigurationManager.Instance.RegionIconConfig;
            _canvas = GetComponentInParent<Canvas>();
        }

        public void SetGood(Good good)
        {
            var newTooltipObject = Instantiate(tooltip, _canvas.transform);
            var newTooltip = newTooltipObject.GetComponent<GoodTooltip>();
            var goodData = _goodsConfig.ConfigData[good];
            var tier = goodData.Tier;
            var tierIcon = _tierIcons.Icons[tier];
            var regionIcon = _regionIcons.GetIcon(goodData.Regions);
            newTooltip.SetUp(goodData.GoodName, _goodsConfig.BasePriceData[tier], tierIcon, regionIcon);
        }
    }
}