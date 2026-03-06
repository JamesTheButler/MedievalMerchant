using System;
using Common.Types;
using Common.Utility;
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
            insufficientAmountYou,
            insufficientAmountTown;

        public string NoTownSelected()
        {
            return noTownSelected.GetLocalizedString();
        }

        public string WrongTownSelected(string townName)
        {
            var dataObject = new { TownName = townName };
            return wrongTownSelected.GetLocalizedString(dataObject);
        }

        public string GoodProducedInTown(string townName, string goodName)
        {
            var dataObject = new { TownName = townName, GoodName = goodName };
            return goodProducedInTown.GetLocalizedString(dataObject);
        }

        public string InsufficientTier(string townName, string goodName, Tier tier)
        {
            var dataObject = new { TownName = townName, GoodName = goodName, Tier = tier.ToRomanNumeral() };
            return insufficientTier.GetLocalizedString(dataObject);
        }

        public string InsufficientGoodYou(string townName, string goodName)
        {
            var dataObject = new { TownName = townName, GoodName = goodName };
            return insufficientGoodYou.GetLocalizedString(dataObject);
        }

        public string InsufficientGoodTown(string goodName)
        {
            var dataObject = new { GoodName = goodName };
            return insufficientGoodTown.GetLocalizedString(dataObject);
        }

        public string InsufficientAmountYou(string townName, string goodName)
        {
            var dataObject = new { TownName = townName, GoodName = goodName };
         return   insufficientAmountYou.GetLocalizedString(dataObject);
        }

        public string InsufficientAmountTown(string goodName)
        {
            var dataObject = new { GoodName = goodName };
           return  insufficientAmountTown.GetLocalizedString(dataObject);
        }
    }
}