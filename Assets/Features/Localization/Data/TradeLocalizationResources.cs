using System;
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
        public TradeFailureStrings FailureStrings { get; private set; }
    }
}