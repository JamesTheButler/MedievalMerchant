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

        [SerializeField, Required]
        private PopupOpenCloseAnimatorHandler animatorHandler;

        private void Awake()
        {
            animatorHandler.OnClosed += OnClosedAnimationCompleted;
        }

        private void OnClosedAnimationCompleted()
        {
            gameObject.SetActive(false);
        }

        public void Show(string message, Action onNextClick)
        {
            explainerText.text = message;
            explainerText.alpha = 1f;
            nextButton.onClick.AddListener(() => onNextClick?.Invoke());
            gameObject.SetActive(true);
            animatorHandler.StartOpenAnimation();
        }

        public void Hide()
        {
            explainerText.alpha = 0f;
            nextButton.onClick.RemoveAllListeners();
            animatorHandler.StartCloseAnimation();
        }
    }
}