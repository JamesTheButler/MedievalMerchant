using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Tutorial.Onboarding.UI
{
    public sealed class OnboardingExplainerUI : MonoBehaviour
    {
        [SerializeField, Required]
        public TMP_Text explainerText;

        [SerializeField, Required]
        private Button nextButton;

        public void Show(string message, Action onNextClick)
        {
            explainerText.text = message;
            nextButton.onClick.AddListener(() => onNextClick?.Invoke());
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            explainerText.text = string.Empty;
            nextButton.onClick.RemoveAllListeners();
            gameObject.SetActive(false);
        }
    }
}