using Common.Infrastructure;
using Common.Types;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionMissionGoodItem : CompanionMissionItem
    {
        public Good Good { get; }
        public float SubstituteCostSingle { get; }

        public CompanionMissionGoodItem(Good good, int targetAmount) : base(targetAmount)
        {
            var companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            var goodResources = ResourceManager.Instance.GoodResources;
            var goodConfig = ConfigurationManager.Configurations.GoodConfig;

            Good = good;
            var tier = goodResources.ResourceData[good].Tier;

            SubstituteCostSingle = goodConfig.BasePriceData[tier] * companionConfig.GoodMissionSubstituteFactor;
        }
    }
}