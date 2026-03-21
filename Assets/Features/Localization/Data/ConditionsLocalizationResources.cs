using System;
using Common.Types;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class ConditionsLocalizationResources
    {
        [SerializeField]
        private LocalizedString
            timeoutProgress,
            bankruptcyProgress,
            fundsProgress,
            globalRepProgress,
            localRepProgress,
            townTierProgress;

        public string TimeoutProgress(int days)
        {
            return timeoutProgress.GetLocalizedString(days);
        }

        public string BankruptcyProgress(int days)
        {
            return bankruptcyProgress.GetLocalizedString(days);
        }

        public string FundsProgress(int current, int max)
        {
            var args = new
            {
                _int_Current = current,
                _int_Max = max,
            };
            return fundsProgress.GetLocalizedString(args);
        }

        public string GlobalRepProgress(int current, int max)
        {
            var args = new
            {
                _int_Current = current,
                _int_Max = max,
            };
            return globalRepProgress.GetLocalizedString(args);
        }

        public string LocalRepProgress(int current, int max)
        {
            var args = new
            {
                _int_Current = current,
                _int_Max = max,
            };
            return localRepProgress.GetLocalizedString(args);
        }

        public string TownTierProgress(int current, int max, Tier tier)
        {
            var args = new
            {
                _int_Current = current,
                _int_Max = max,
                TierRoman = tier,
            };
            return townTierProgress.GetLocalizedString(args);
        }
    }
}