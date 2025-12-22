using Common.Config;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Tooltips;
using Features.Goods.Config;
using Features.Towns;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Goods.UI
{
    public class GoodTooltip : TooltipBase<Good>
    {
        [SerializeField, Required]
        private TMP_Text nameText, priceText, currentPriceText;

        [SerializeField, Required]
        private Image goodImage, tierImage, regionImage;

        private GoodsResources _goodsResources;
        private GoodsConfig _goodsConfig;
        private TierResources _tierIcons;
        private RegionResources _region;
        private Selection _selection;

        private Good _good;
        private Town _selectedTown;
        protected override void Awake()
        {
            base.Awake();
            _goodsConfig = ConfigurationManager.Configurations.GoodsConfig;
            _goodsResources = ResourceManager.Instance.GoodsResources;
            _tierIcons = ResourceManager.Instance.TierResources;
            _region = ResourceManager.Instance.RegionResources;

            _selection = GameplayContext.Instance.Selection;
        }

        protected override void UpdateUI(Good data)
        {
            _good = data;
            
            var goodData = _goodsResources.ConfigData[_good];
            var tier = goodData.Tier;
            var price = _goodsConfig.BasePriceData[tier];
            var tierIcon = _tierIcons.Icons[tier];
            // TODO - HACK: should it take First() instead of random?
            var regionIcon = _region.Data[goodData.Regions.GetRandom()];

            nameText.text = goodData.GoodName;
            priceText.text = $"{price:0.##}";

            goodImage.sprite = goodData.Icon;
            tierImage.sprite = tierIcon;
            regionImage.sprite = regionIcon.Icon;

            _selection.SelectedTown.Observe(OnTownChanged);
        }

        private void OnTownChanged(Town newTown)
        {
            // unobserve the old price
            _selectedTown?.PriceManager.GetPrice(_good)?.StopObserving(OnTownPriceChanged);
            
            _selectedTown = newTown;
            if (newTown == null)
            {
                currentPriceText.text = "-";
            }
            else
            {
                var priceInTown = newTown.PriceManager.GetPrice(_good);
                priceInTown?.Observe(OnTownPriceChanged);
                if (priceInTown == null)
                {
                    currentPriceText.text = "-";
                }
            }
        }

        private void OnTownPriceChanged(float price)
        {
            currentPriceText.text = $"{price:0.#}";
        }

        public override void Reset()
        {
            _selection.SelectedTown.StopObserving(OnTownChanged);
            _selection.SelectedTown.Value?.PriceManager?.GetPrice(_good)?.StopObserving(OnTownPriceChanged);
        }
    }
}