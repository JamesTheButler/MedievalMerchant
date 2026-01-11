using Common.Infrastructure;
using Common.UI.Tooltips;
using Features.Goods.Config;
using Features.Player.Caravan.Config;
using Features.Player.Caravan.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Player.Caravan.UI
{
    public sealed class CartUpgradeTooltip : TooltipBase<Cart>
    {
        [SerializeField, Required]
        private TMP_Text costText;

        [SerializeField, Required]
        private CartUpgradeTooltipDetails currentTierGroup, nextTierGroup;

        private CaravanConfig _caravanConfig;
        private CaravanResources _caravanResources;

        protected override void Awake()
        {
            base.Awake();
            _caravanResources = ResourceManager.Instance.CaravanResources;
            _caravanConfig = ConfigurationManager.Configurations.CaravanConfig;
        }

        protected override void UpdateUI(Cart cart)
        {
            var level = cart.Level.Value;
            var nextLevel = level + 1;

            if (nextLevel > CaravanConfig.MaxLevel)
                return;

            var currentLevelIcon = _caravanResources.TierIcons[level];
            var currentLevelData = _caravanConfig.GetUpgradeData(level);

            currentTierGroup.SetUp(
                currentLevelIcon,
                level,
                currentLevelData.SlotCount,
                currentLevelData.MoveSpeed,
                currentLevelData.Upkeep);

            var nextLevelIcon = _caravanResources.TierIcons[nextLevel];
            var nextLevelData = _caravanConfig.GetUpgradeData(nextLevel);
            nextTierGroup.SetUp(
                nextLevelIcon,
                nextLevel,
                nextLevelData.SlotCount,
                nextLevelData.MoveSpeed,
                nextLevelData.Upkeep);

            nextTierGroup.SetDiffs(
                nextLevelData.SlotCount - currentLevelData.SlotCount,
                nextLevelData.MoveSpeed - currentLevelData.MoveSpeed,
                nextLevelData.Upkeep - currentLevelData.Upkeep
            );

            costText.text = $"Cost: {cart.UpgradeCost.Value:0.#}";
        }

        public override void Reset() { }
    }
}