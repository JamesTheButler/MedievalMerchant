using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Common.Utility
{
    public static class LocalizationExtensions
    {
        public static void SetLocalizedText(this TMP_Text text, LocalizedString localizedString)
        {
            text.text = localizedString.GetLocalizedString();
        }

        public static void SetArguments(this LocalizeStringEvent localizer, params object[] args)
        {
            localizer.StringReference.Arguments = args;
            localizer.RefreshString();
        }

        public static void SetSmartArgument(this LocalizeStringEvent localizer, string name, int value)
        {
            localizer.StringReference.Add(name, new IntVariable { Value = value });
            localizer.RefreshString();
        }

        public static void SetSmartArguments(this LocalizeStringEvent localizer, params (string Key, int Value)[] args)
        {
            foreach (var (key, value) in args)
            {
                localizer.StringReference.Add(key, new IntVariable { Value = value });
            }

            localizer.RefreshString();
        }
    }
}