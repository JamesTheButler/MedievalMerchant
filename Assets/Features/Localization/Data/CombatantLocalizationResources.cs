using System;
using Common.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class CombatantLocalizationResources
    {
        [SerializeField]
        private LocalizedString teamName,
            unitName;

        public string TeamName => teamName.GetLocalizedStringOptional();
        public string UnitName => unitName.GetLocalizedString();
    }
}