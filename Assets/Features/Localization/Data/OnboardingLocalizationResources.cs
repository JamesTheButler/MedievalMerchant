using System;
using Common.Types;
using Common.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class OnboardingLocalizationResources
    {
        [SerializeField]
        private LocalizedString
            travelToTask,
            buildProducerTask,
            unpauseGameTask,
            setSpeedTask,
            upgradeCartTask,
            sellGoodsTask,
            buyGoodsTask,
            upgradeTownTask;

        public string TravelToTask(string townName)
        {
            return travelToTask.GetLocalizedString(new { TownName = townName });
        }

        public string BuildProducerTask(string townName, string producerName)
        {
            var stringObj = new
            {
                TownName = townName,
                ProducerName = producerName
            };

            return buildProducerTask.GetLocalizedString(stringObj);
        }

        public string UnpauseGameTask()
        {
            return unpauseGameTask.GetLocalizedString();
        }

        public string SetSpeedTask()
        {
            return setSpeedTask.GetLocalizedString();
        }

        public string UpgradeCartTask(Tier tier)
        {
            return upgradeCartTask.GetLocalizedString(new { Tier = tier.ToRomanNumeral() });
        }

        public string SellGoodsTask(int amount, string goodName, string townName)
        {
            var stringObj = new
            {
                _int_Amount = amount,
                GoodName = goodName,
                TownName = townName
            };

            return sellGoodsTask.GetLocalizedString(stringObj);
        }

        public string BuyGoodsTask(int amount, string goodName, string townName)
        {
            var stringObj = new
            {
                _int_Amount = amount,
                GoodName = goodName,
                TownName = townName
            };

            return buyGoodsTask.GetLocalizedString(stringObj);
        }

        public string UpgradeTownTask(string townName, Tier tier)
        {
            var stringObj = new
            {
                TownName = townName,
                Tier = tier.ToRomanNumeral()
            };

            return upgradeTownTask.GetLocalizedString(stringObj);
        }
    }
}