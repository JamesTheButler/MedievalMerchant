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

        private void Awake()
        {
            englishToggle.onValueChanged.AddListener(_ => SetLocale(englishLocale));
            frenchToggle.onValueChanged.AddListener(_ => SetLocale(frenchLocale));
        }

        private void SetLocale(Locale locale)
        {
            if (LocalizationSettings.SelectedLocale == locale)
                return;

            LocalizationSettings.SelectedLocale = locale;
            SceneManager.LoadScene(startScene);
        }

        protected override void OnOpen()
        {
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