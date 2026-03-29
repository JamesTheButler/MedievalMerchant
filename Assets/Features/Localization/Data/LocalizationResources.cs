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
        private LocalizedString date, perDay, cost, and;

        [field: SerializeField]
        public ConditionsLocalizationResources Conditions { get; private set; }

        [field: SerializeField]
        public GoodLocalizationResources Goods { get; private set; }

        [field: SerializeField]
        public MissionLocalizationResources Missions { get; private set; }

        [field: SerializeField]
        public ModifierLocalizationResources Modifiers { get; private set; }

        [field: SerializeField]
        public NotificationLocalizationResources Notifications { get; private set; }

        [field: SerializeField]
        public PlayerLocalizationResources Player { get; private set; }

        [field: SerializeField]
        public TradeLocalizationResources Trade { get; private set; }

        [field: SerializeField]
        public TownLocalizationResources Town { get; private set; }

        [field: SerializeField]
        public OnboardingLocalizationResources Onboarding { get; private set; }

        public string Date(Date value)
        {
            var args = new
            {
                _int_Day = value.Day,
                _int_Year = value.Year,
            };

            return date.GetLocalizedString(args);
        }

        public string PerDay(string value)
        {
            return perDay.GetLocalizedString(value);
        }

        public string Cost(float value)
        {
            var args = new { _float_Cost = value };
            return cost.GetLocalizedString(args);
        }

        public string And => and.GetLocalizedString();
    }
}