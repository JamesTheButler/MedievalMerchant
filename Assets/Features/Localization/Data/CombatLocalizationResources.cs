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