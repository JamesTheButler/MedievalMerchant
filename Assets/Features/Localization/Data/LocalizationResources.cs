using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using Features.Localization.UI;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [CreateAssetMenu(
        fileName = nameof(LocalizationResources),
        menuName = AssetMenu.ResourceFolder + nameof(LocalizationResources))]
    public sealed class LocalizationResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Difficulty, LocalizedString> Difficulties { get; private set; }

        [SerializeField]
        private LocalizedString date;

        [field: SerializeField]
        public MissionLocalizationResources MissionStrings { get; private set; }

        [field: SerializeField]
        public NotificationLocalizationResources NotificationResources { get; private set; }

        [field: SerializeField]
        public PlayerLocalizationResources Player { get; private set; }

        [field: SerializeField]
        public TradeLocalizationResources TradeStrings { get; private set; }

        [field: SerializeField]
        public TownLocalizationResources Town { get; private set; }

        [field: SerializeField]
        public OnboardingLocalizationResources OnboardingResources { get; private set; }

        public string Date(Date value)
        {
            var args = new
            {
                _int_Day = value.Day,
                _int_Year = value.Year,
            };

            return date.GetLocalizedString(args);
        }
    }
}