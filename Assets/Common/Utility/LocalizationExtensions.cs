using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Common.Utility
{
    public static class LocalizationExtensions
    {
        public static void SetText(this TMP_Text text, LocalizedString localizedString)
        {
            text.text = localizedString.GetLocalizedString();
        }

        public static void SetArguments(this LocalizeStringEvent localizer, params object[] args)
        {
            localizer.StringReference.Arguments = args;
            localizer.RefreshString();
        }
    }
}