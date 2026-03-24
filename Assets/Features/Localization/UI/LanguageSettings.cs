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
        [SerializeField, Scene]
        private string startScene;

        [SerializeField, Required]
        private Toggle englishToggle, frenchToggle;

        [SerializeField, Required]
        private Locale englishLocale, frenchLocale;

        private bool _isChangingLocale;

        private void Awake()
        {
            englishToggle.onValueChanged.AddListener(isOn => { if (isOn) SetLocale(englishLocale); });
            frenchToggle.onValueChanged.AddListener(isOn => { if (isOn) SetLocale(frenchLocale); });
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
            yield return LocalizationSettings.SelectedLocaleAsync;
            SceneManager.LoadScene(startScene);
        }

        protected override void OnOpen()
        {
            _isChangingLocale = false;

            var locale = LocalizationSettings.SelectedLocale;

            if (locale == frenchLocale)
            {
                frenchToggle.SetIsOnWithoutNotify(true);
            }
            else
            {
                englishToggle.SetIsOnWithoutNotify(true);
            }

            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}