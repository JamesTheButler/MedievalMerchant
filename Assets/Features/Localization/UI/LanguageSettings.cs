using System;
using System.Collections;
using Common.Infrastructure.Global;
using Common.UI.Elements.Panels;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.Localization.UI
{
    public sealed class LanguageSettingsUI : DynamicPanel
    {
        [Serializable]
        private sealed class LanguageOption
        {
            [field: SerializeField, Required]
            public Toggle Toggle { get; private set; }

            [field: SerializeField, Required]
            public Locale Locale { get; private set; }
        }

        [SerializeField, Scene]
        private string startScene;

        [SerializeField]
        private LanguageOption[] languages;

        private bool _isChangingLocale;

        private void Awake()
        {
            foreach (var language in languages)
            {
                var locale = language.Locale;
                language.Toggle.onValueChanged.AddListener(isOn =>
                {
                    if (isOn)
                    {
                        SetLocale(locale);
                    }
                });
            }
        }

        private void SetLocale(Locale locale)
        {
            if (_isChangingLocale)
                return;

            if (LocalizationSettings.SelectedLocale == locale)
                return;

            _isChangingLocale = true;
            StartCoroutine(ChangeLocaleAndReload(locale));
        }

        private IEnumerator ChangeLocaleAndReload(Locale locale)
        {
            yield return LocalizationSettings.InitializationOperation;
            GlobalContext.Instance.Services.LocaleService.SaveLocale(locale);
            LocalizationSettings.SelectedLocale = locale;
            yield return LocalizationSettings.StringDatabase.GetAllTables(locale);
            SceneManager.LoadScene(startScene);
        }

        protected override void OnOpen()
        {
            _isChangingLocale = false;

            var selectedLocale = LocalizationSettings.SelectedLocale;
            var selectedToggle = languages[0].Toggle;

            foreach (var language in languages)
            {
                if (language.Locale != selectedLocale)
                    continue;

                selectedToggle = language.Toggle;
                break;
            }

            selectedToggle.SetIsOnWithoutNotify(true);

            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}
