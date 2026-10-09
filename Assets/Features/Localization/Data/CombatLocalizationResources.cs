using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class CombatLocalizationResources
    {
        [SerializeField]
        private LocalizedString unitLossOutOf;

        [field: SerializeField]
        public LocalizedString GuardsTeamName { get; private set; }

        [field: SerializeField]
        public LocalizedString GuardsUnitName { get; private set; }

        [field: SerializeField]
        public LocalizedString BanditsTeamName { get; private set; }

        [field: SerializeField]
        public LocalizedString BanditsUnitName { get; private set; }

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