using System;
using System.Text;
using Common.Infrastructure;
using Common.UI.Utility;
using Common.Utility;
using Features.Player.Retinue.Config.Resources;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public sealed class ThiefLevelData : CompanionLevelData
    {
        [field: SerializeField]
        public float TownEntranceGold { get; private set; }

        [field: SerializeField, Range(0f, 1f)]
        public float ReputationLossChance { get; private set; }

        [field: SerializeField, Range(0, 100)]
        public float ReputationLoss { get; private set; }

        private ThiefCompanionResource Resource => ResourceManager.Instance.CompanionResources.Thief;
        
        public override string Description => new StringBuilder()
            .AppendLine(Resource.TownEntranceGoldString.GetLocalizedString(TownEntranceGold))
            .AppendLine(Resource.ReputationLossChanceString.GetLocalizedString(ReputationLossChance.ToPercentString()).WithStyle(Style.Bad))
            .AppendLine(Resource.ReputationLossString.GetLocalizedString(ReputationLoss).WithStyle(Style.Bad))
            .ToString();
    }
}