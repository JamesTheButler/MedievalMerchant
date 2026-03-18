using System;
using System.Text;
using Common.Infrastructure;
using Common.Utility;
using Features.Player.Retinue.Config.Resources;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public sealed class NegotiatorLevelData : CompanionLevelData
    {
        [field: SerializeField]
        public float PriceSavings { get; private set; }

        [field: SerializeField]
        public float UpgradeCostReduction { get; private set; }

        private NegotiatorCompanionResource Resource => ResourceManager.Instance.CompanionResources.Negotiator;

        public override string Description => new StringBuilder()
            .AppendLine($"- {Resource.PriceSavingsString.GetLocalizedString(PriceSavings.ToPercentString())}")
            .AppendLine($"- {Resource.UpgradeCostReductionString.GetLocalizedString(UpgradeCostReduction.ToPercentString())}")
            .ToString();
    }
}