using System;
using Common.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class TownMilestonesLocalizationResources
    {
        [SerializeField]
        private LocalizedString selfSufficiency,
            fundsBoost,
            productionBoost,
            priceBoost,
            dividends;

        public string SelfSufficiency()
        {
            return selfSufficiency.GetLocalizedString();
        }

        public string FundsBoost(float percentage)
        {
            return fundsBoost.GetLocalizedString(percentage.ToPercentString());
        }

        public string ProductionBoost(float percentage)
        {
            return productionBoost.GetLocalizedString(percentage.ToPercentString());
        }

        public string PriceBoost(float percentage)
        {
            return priceBoost.GetLocalizedString(percentage.ToPercentString());
        }

        public string Dividends(float percentage)
        {
            return dividends.GetLocalizedString(percentage.ToPercentString());
        }
    }
}