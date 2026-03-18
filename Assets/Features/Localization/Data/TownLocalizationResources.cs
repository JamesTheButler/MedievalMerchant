using System;
using Common.Types;
using Common.Utility;
using Features.Localization.UI;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class TownLocalizationResources
    {
        [field: SerializeField]
        public LocalizedString DevBaseRate { get; private set; }

        [field: SerializeField]
        public LocalizedString DevTrendModifierTitle { get; private set; }

        [field: SerializeField]
        public LocalizedString FundsChangeBaseRate { get; private set; }

        [field: SerializeField]
        public LocalizedString FundsChangeModifierTitle { get; private set; }

        [field: SerializeField]
        public TownMilestonesLocalizationResources Milestones { get; private set; }

        [SerializeField]
        private LocalizedString storeGoodsModifier, producerModifier, dividendsModifier;

        public string StoredGoodsDevelopmentModifier(int goodCount, Tier tier)
        {
            var args = new
            {
                _int_GoodCount = goodCount,
                TierRoman = tier
            };

            return storeGoodsModifier.GetLocalizedString(args);
        }
        public string ProducerDevelopmentModifier(int producerCount, Tier tier)
        {
            var args = new
            {
                _int_Amount = producerCount,
                TierRoman = tier
            };

            return producerModifier.GetLocalizedString(args);
        }
        public string DividendsFundsModifier(float percentage, string townName)
        {
            var args = new
            {
                Percentage = percentage.ToPercentString(),
                TownName = townName
            };

            return dividendsModifier.GetLocalizedString(args);
        }
    }
}