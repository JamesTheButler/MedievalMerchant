using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class PlayerLocalizationResources
    {
        [SerializeField]
        private LocalizedString fundsChangeModifier,
            movementSpeed,
            caravanUpkeep,
            retinueUpkeep,
            upkeepBase,
            upgradeCost,
            upgradeCostBase,
            cartUpkeep,
            average;

        [field: SerializeField]
        public CompanionLocalizationResources Companions { get; private set; }

        public string FundsChangeModifier => fundsChangeModifier.GetLocalizedString();
        public string MovementSpeed => movementSpeed.GetLocalizedString();
        public string CaravanUpkeep => caravanUpkeep.GetLocalizedString();
        public string RetinueUpkeep => retinueUpkeep.GetLocalizedString();
        public string UpkeepBase => upkeepBase.GetLocalizedString();
        public string UpgradeCost => upgradeCost.GetLocalizedString();

        public string UpgradeCostBase(int level) => upgradeCostBase.GetLocalizedString(level);

        public string CartUpkeep(int cartIndex, int level)
        {
            var args = new
            {
                _int_Level = level,
                _int_Index = cartIndex,
            };
            return cartUpkeep.GetLocalizedString(args);
        }

        // e.g. Average movement speed
        public string Average(string attribute)
        {
            return average.GetLocalizedString(attribute.ToLower());
        }
    }
}