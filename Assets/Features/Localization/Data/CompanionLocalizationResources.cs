using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class CompanionLocalizationResources
    {
        [SerializeField]
        private LocalizedString levelString;

        public string CompanionDisplayString(string companionName, int level)
        {
            var args = new
            {
                CompanionName = companionName,
                _int_Level = level
            };
            return levelString.GetLocalizedString(args);
        }
    }
}