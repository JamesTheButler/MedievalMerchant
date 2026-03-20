using System;
using Common.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class ModifierLocalizationResources
    {
        [SerializeField]
        private LocalizedString allyEffect,
            foeEffect,
            allyEffectDescription,
            developmentEffect,
            missionLimiterEffect,
            movementSpeedEffect,
            priceEffect,
            buyPriceEffect,
            sellPriceEffect,
            productionEffect,
            reputationEffect;

        public string AllyEffectDescription => allyEffect.GetLocalizedString();
        public string AllyEffect => allyEffect.GetLocalizedString();
        public string FoeEffect => allyEffect.GetLocalizedString();

        public string DevelopmentEffect(float value)
        {
            var args = new
            {
                Percentage = value.ToPercentString(true)
            };
            return developmentEffect.GetLocalizedString(args);
        }

        public string MissionLimiterEffect(string regionName)
        {
            var args = new
            {
                RegionName = regionName
            };
            return missionLimiterEffect.GetLocalizedString(args);
        }

        public string MovementSpeedEffect(float value)
        {
            var args = new
            {
                Percentage = value.ToPercentString(true)
            };
            return movementSpeedEffect.GetLocalizedString(args);
        }

        public string PriceEffect(float value, string selector)
        {
            var args = new
            {
                Percentage = value.ToPercentString(true),
                GoodSelector = selector
            };
            return priceEffect.GetLocalizedString(args);
        }

        public string BuyPriceEffect(float value, string selector)
        {
            var args = new
            {
                Percentage = value.ToPercentString(true),
                GoodSelector = selector
            };
            return buyPriceEffect.GetLocalizedString(args);
        }

        public string SellPriceEffect(float value, string selector)
        {
            var args = new
            {
                Percentage = value.ToPercentString(true),
                GoodSelector = selector
            };
            return sellPriceEffect.GetLocalizedString(args);
        }

        public string ProductionEffect(float value, string selector)
        {
            var args = new
            {
                Percentage = value.ToPercentString(true),
                GoodSelector = selector
            };
            return productionEffect.GetLocalizedString(args);
        }

        public string ReputationEffect(float value)
        {
            var args = new
            {
                Percentage = value.ToPercentString(true)
            };
            return reputationEffect.GetLocalizedString(args);
        }
    }
}