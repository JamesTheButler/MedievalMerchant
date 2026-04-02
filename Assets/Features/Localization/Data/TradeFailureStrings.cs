using System;
using Common.Infrastructure;
using Common.Types;
using Common.Utility;
using Features.Goods.Config;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class TradeFailureStrings
    {
        [SerializeField]
        private LocalizedString noTownSelected,
            wrongTownSelected,
            goodProducedInTown,
            insufficientTier,
            insufficientGoodYou,
            insufficientGoodTown,
            insufficientGoodsCamp,
            insufficientAmountYou,
            insufficientAmountTown,
            insufficientSlots,
            insufficientSpace;

        private Lazy<GoodResources> _goodResources = new(() => ResourceManager.Instance.GoodResources);

        private string GoodName(Good good)
        {
            return _goodResources.Value.ResourceData[good].GoodName;
        }

        public string NoTownSelected()
        {
            return noTownSelected.GetLocalizedString();
        }

        public string WrongTownSelected(string townName)
        {
            var dataObject = new { TownName = townName };
            return wrongTownSelected.GetLocalizedString(dataObject);
        }

        public string GoodProducedInTown(string townName, Good good)
        {
            var dataObject = new { TownName = townName, GoodName = GoodName(good) };
            return goodProducedInTown.GetLocalizedString(dataObject);
        }

        public string InsufficientTier(string townName, Good good, Tier tier)
        {
            var dataObject = new { TownName = townName, GoodName = GoodName(good), Tier = tier.ToRomanNumeral() };
            return insufficientTier.GetLocalizedString(dataObject);
        }

        public string InsufficientGoodYou(Good good)
        {
            var dataObject = new { GoodName = GoodName(good) };
            return insufficientGoodYou.GetLocalizedString(dataObject);
        }

        public string InsufficientGoodTown(string townName, Good good)
        {
            var dataObject = new { TownName = townName, GoodName = GoodName(good) };
            return insufficientGoodTown.GetLocalizedString(dataObject);
        }

        public string InsufficientAmountYou(Good good)
        {
            var dataObject = new { GoodName = GoodName(good) };
            return insufficientAmountYou.GetLocalizedString(dataObject);
        }

        public string InsufficientAmountTown(string townName, Good good)
        {
            var dataObject = new { TownName = townName, GoodName = GoodName(good) };
            return insufficientAmountTown.GetLocalizedString(dataObject);
        }

        public string InsufficientSlots(Tier tier)
        {
            var dataObject = new { Tier = tier.ToRomanNumeral() };
            return insufficientSlots.GetLocalizedString(dataObject);
        }

        public string InsufficientGoodsCamp(Good good)
        {
            var dataObject = new { GoodName = GoodName(good) };
            return insufficientGoodsCamp.GetLocalizedString(dataObject);
        }

        public string InsufficientSpace()
        {
            return insufficientSpace.GetLocalizedString();
        }
    }
}