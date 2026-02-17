using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Common.Utility
{
    public static class LocalizationExtensions
    {
        public static void SetText(this TMP_Text text, LocalizedString localizedString)
        {
            text.text = localizedString.GetLocalizedString();
        }

        public static void Update(this LocalizeStringEvent localizer, LocalizedString localizedString)
        {
            localizer.StringReference = localizedString;
            localizer.RefreshString();
        }

        public static void SetArguments(this LocalizeStringEvent localizer, params object[] args)
        {
            localizer.StringReference.Arguments = args;
            localizer.RefreshString();
        }

        public static void SetArgument(this LocalizeStringEvent localizer, string name, int value)
        {
            localizer.StringReference.Add(name, new IntVariable { Value = value });
            localizer.RefreshString();
        }
    }
}