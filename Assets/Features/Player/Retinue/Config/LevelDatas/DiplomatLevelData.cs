using System;
using System.Text;
using Common.Infrastructure;
using Common.Utility;
using Features.Player.Retinue.Config.Resources;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public sealed class DiplomatLevelData : CompanionLevelData
    {
        [field: SerializeField, Range(0, 100)]
        public float TownEntranceReputation { get; private set; }

        [field: SerializeField]
        public float ReputationBoost { get; private set; }

        private DiplomatCompanionResource Resource => ResourceManager.Instance.CompanionResources.Diplomat;

        public override string Description => new StringBuilder()
            .AppendLine($"- {Resource.TownEntranceRepString.GetLocalizedString(TownEntranceReputation)}")
            .AppendLine($"- {Resource.RepBoostString.GetLocalizedString(ReputationBoost.ToPercentString())}")
            .ToString();
    }
}