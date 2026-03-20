using System;
using Common.Types;
using Common.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class TradeLocalizationResources
    {
        [field: SerializeField]
        public LocalizedString BuyString { get; private set; }

        [field: SerializeField]
        public LocalizedString SellString { get; private set; }

        [field: SerializeField]
        public LocalizedString NetLossString { get; private set; }

        [field: SerializeField]
        public LocalizedString NetProfitString { get; private set; }

        [field: SerializeField]
        public LocalizedString YouNotEnoughCoin { get; private set; }

        [field: SerializeField]
        public LocalizedString TownNotEnoughCoin { get; private set; }

        [field: SerializeField]
        public LocalizedString FundsSummary { get; private set; }

        [field: SerializeField]
        public LocalizedString ReputationSummary { get; private set; }

        [field: SerializeField]
        public LocalizedString PricePerGood { get; private set; }

        [field: SerializeField]
        public LocalizedString ReputationLikeModifier { get; private set; }

        [field: SerializeField]
        public LocalizedString ReputationDislikeModifier { get; private set; }

        [SerializeField]
        private LocalizedString availabilityLabel,
            disinterestModifier,
            globalSurplusModifier,
            foreignGoodModifier,
            localGoodModifier,
            priceBase;

        [field: SerializeField]
        public TradeFailureStrings FailureStrings { get; private set; }

        public string ForeignGoodModifier => foreignGoodModifier.GetLocalizedString();
        public string LocalGoodModifier => localGoodModifier.GetLocalizedString();

        public string AvailabilityLabel(string availability)
        {
            return availabilityLabel.GetLocalizedString(availability);
        }

        public string DisinterestDescription(
            string goodName,
            int amount,
            int purchasePeriodInDays,
            float percentPerStep,
            int goodsPerStep)
        {
            var args = new
            {
                GoodName = goodName,
                _int_Amount = amount,
                _int_Days = purchasePeriodInDays,
                Percentage = percentPerStep.ToPercentString(),
                _int_StepSize = goodsPerStep,
            };
            return disinterestModifier.GetLocalizedString(args);
        }

        public string GlobalSurplusDescription(int amount, string goodName, float reduction, int stepSize)
        {
            var args = new
            {
                _int_Amount = amount,
                GoodName = goodName,
                Percentage = reduction.ToPercentString(),
                _int_StepSize = stepSize
            };
            return globalSurplusModifier.GetLocalizedString(args);
        }

        public string BasePrice(Tier tier)
        {
            var args = new { TierRoman = tier.ToRomanNumeral() };
            return priceBase.GetLocalizedString(args);
        }
    }
}