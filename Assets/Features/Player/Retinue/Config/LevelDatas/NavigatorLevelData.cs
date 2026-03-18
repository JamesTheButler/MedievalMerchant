using System;
using System.Text;
using Common.Infrastructure;
using Common.Utility;
using Features.Player.Retinue.Config.Resources;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public sealed class NavigatorLevelData : CompanionLevelData
    {
        [field: SerializeField]
        public float SpeedBonus { get; private set; }

        [field: SerializeField]
        public float UpkeepReduction { get; private set; }

        private NavigatorCompanionResource Resource => ResourceManager.Instance.CompanionResources.Navigator;
        
        public override string Description => new StringBuilder()
            .AppendLine($"- {Resource.SpeedBonusString.GetLocalizedString(SpeedBonus.ToPercentString())}")
            .AppendLine($"- {Resource.UpkeepReductionString.GetLocalizedString(UpkeepReduction.ToPercentString())}")
            .ToString();
    }
}