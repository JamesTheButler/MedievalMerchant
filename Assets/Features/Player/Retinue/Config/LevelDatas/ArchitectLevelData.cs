using System;
using System.Text;
using Common.Infrastructure;
using Common.Utility;
using Features.Player.Retinue.Config.Resources;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public sealed class ArchitectLevelData : CompanionLevelData
    {
        [field: SerializeField, Range(0f, 1f)]
        public float ConstructionPriceReduction { get; private set; }

        private ArchitectCompanionResource Resource => ResourceManager.Instance.CompanionResources.Architect;
        
        public override string Description => new StringBuilder()
            .AppendLine(Resource.CostReductionString.GetLocalizedString(ConstructionPriceReduction.ToPercentString()))
            .ToString();
    }
}