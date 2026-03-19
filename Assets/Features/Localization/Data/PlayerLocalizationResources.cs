using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class PlayerLocalizationResources
    {
        [SerializeField]
        private LocalizedString fundsChangeModifier, movementSpeed, caravanUpkeep, retinueUpkeep, upgradeCost, cartUpkeep;

        [field: SerializeField]
        public CompanionLocalizationResources Companions { get; private set; }

        public string FundsChangeModifier => fundsChangeModifier.GetLocalizedString();
        public string MovementSpeed => movementSpeed.GetLocalizedString();
        public string CaravanUpkeep => caravanUpkeep.GetLocalizedString();
        public string RetinueUpkeep => retinueUpkeep.GetLocalizedString();
        public string UpgradeCost => upgradeCost.GetLocalizedString();
        
        public string CartUpkeep(int level) => cartUpkeep.GetLocalizedString(level);
    }
}