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
        public float Health { get; private set; }

        [field: SerializeField]
        public float CombatStrength { get; private set; }

        [field: SerializeField]
        public int MaxGuardCount { get; private set; }

        [field: SerializeField]
        public int HireCostPerGuard { get; private set; }

        private GuardCompanionResource Resource => ResourceManager.Instance.CompanionResources.Guard;

        public override string Description => new StringBuilder()
            .AppendLine(Resource.StrengthString.GetLocalizedString(CombatStrength))
            .ToString();
    }
}