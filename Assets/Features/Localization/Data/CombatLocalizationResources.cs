using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class CombatLocalizationResources
    {
        [SerializeField]
        private LocalizedString unitLossOutOf,
            healthPerUnit,
            strengthPerUnit,
            totalHealth,
            totalStrength;

        public string HealthPerUnit => healthPerUnit.GetLocalizedString();
        public string StrengthPerUnit => strengthPerUnit.GetLocalizedString();
        public string TotalHealth => totalHealth.GetLocalizedString();
        public string TotalStrength => totalStrength.GetLocalizedString();

        [field: SerializeField]
        public CombatantLocalizationResources Guards { get; private set; }

        [field: SerializeField]
        public CombatantLocalizationResources Bandits { get; private set; }

        public string UnitLossOutOf(int maxUnitCount)
        {
            var args = new
            {
                _int_MaxUnitCount = maxUnitCount,
            };
            return unitLossOutOf.GetLocalizedString(args);
        }
    }
}