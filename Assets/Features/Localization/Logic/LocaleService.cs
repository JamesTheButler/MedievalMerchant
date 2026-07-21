using Common.Infrastructure;
using Common.Infrastructure.Global;
using Features.Localization.Data;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Core.Settings;

namespace Features.Localization.Logic
{
    public sealed class LocaleService : IService
    {
        private LocalePersistenceService _persistence;

        public void Initialize()
        {
            _persistence = GlobalContext.Instance.PersistenceServices.LocalePersistenceService;
            ApplyStartupLocale();

#if !UNITY_EDITOR
            ConfigureBuild();
#endif
        }

        [UsedImplicitly]
        private static void ConfigureBuild()
        {
            // Use fallback locale in builds. Use the placeholder in Editor for easier debugging.
            LocalizationSettings.StringDatabase.UseFallback = true;
            LocalizationSettings.AssetDatabase.UseFallback = true;

            // Reduce error logs for missing smart format parameters in build
            var smartSettings = LocalizationSettings.StringDatabase.SmartFormatter.Settings;
            smartSettings.FormatErrorAction = ErrorAction.MaintainTokens;
            smartSettings.ParseErrorAction = ErrorAction.MaintainTokens;
        }

        public void CleanUp() { }

        public void SaveLocale(Locale locale)
        {
            _persistence.Save(new LocaleSaveData(locale.Identifier.Code));
        }

        private void ApplyStartupLocale()
        {
            if (_persistence.HasData())
            {
                var saved = _persistence.Load();
                var locale = FindLocale(saved.LocaleCode);
                if (locale != null)
                {
                    LocalizationSettings.SelectedLocale = locale;
                    return;
                }
            }

            var systemLocale = FindLocale(Application.systemLanguage) ?? GetFallbackLocale();
            LocalizationSettings.SelectedLocale = systemLocale;
            SaveLocale(systemLocale);
        }

        private static Locale FindLocale(string code)
        {
            return LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(code));
        }

        private static Locale FindLocale(SystemLanguage language)
        {
            return LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(language));
        }

        private static Locale GetFallbackLocale()
        {
            return FindLocale("en") ?? LocalizationSettings.AvailableLocales.Locales[0];
        }
    }
}