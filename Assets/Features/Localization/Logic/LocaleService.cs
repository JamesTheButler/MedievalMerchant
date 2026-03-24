using Common.Infrastructure;
using Common.Infrastructure.Global;
using Features.Localization.Data;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Features.Localization.Logic
{
    public sealed class LocaleService : IService
    {
        private LocalePersistenceService _persistence;

        public void Initialize()
        {
            _persistence = GlobalContext.Instance.PersistenceServices.LocalePersistenceService;
            ApplyStartupLocale();
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