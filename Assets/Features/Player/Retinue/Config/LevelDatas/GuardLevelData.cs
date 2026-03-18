using System;
using System.Text;
using Common.Infrastructure;
using Features.Player.Retinue.Config.Resources;
using UnityEngine;

namespace Features.Player.Retinue.Config.LevelDatas
{
    [Serializable]
    public sealed class GuardLevelData : CompanionLevelData
    {
        [field: SerializeField]
        public int Strength { get; private set; }

        private GuardCompanionResource Resource => ResourceManager.Instance.CompanionResources.Guard;
        
        public override string Description => new StringBuilder()
            .AppendLine($"- {Resource.StrengthString.GetLocalizedString(Strength)}")
            .ToString();
    }
}