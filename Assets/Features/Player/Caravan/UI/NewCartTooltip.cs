using Common.Infrastructure;
using Common.UI.Tooltips;
using Features.Goods.Config;
using Features.Localization.Data;
using Features.Player.Caravan.Config;
using Features.Player.Caravan.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Player.Caravan.UI
{
    public sealed class NewCartTooltip : TooltipBase<Cart>
    {
        [SerializeField, Required]
        private TMP_Text costText;

        [SerializeField, Required]
        private CartUpgradeTooltipDetails tierGroup;

        private CaravanConfig _caravanConfig;
        private CaravanResources _caravanResources;
        private LocalizationResources _loc;

        protected override void Awake()
        {
            base.Awake();

            _loc = ResourceManager.Instance.LocalizationResources;
            _caravanResources = ResourceManager.Instance.CaravanResources;
            _caravanConfig = ConfigurationManager.Configurations.CaravanConfig;
        }

        protected override void UpdateUI(Cart cart)
        {
            if (cart.Level.Value != 0)
                return;

            var levelOneData = _caravanConfig.GetUpgradeData(1);

            tierGroup.SetUp(
                _caravanResources.TierIcons[1],
                1,
                levelOneData.SlotCount,
                levelOneData.MoveSpeed,
                levelOneData.Upkeep);

            costText.text = _loc.Cost(cart.UpgradeCost.Value);
        }

        public override void Reset() { }
    }
}