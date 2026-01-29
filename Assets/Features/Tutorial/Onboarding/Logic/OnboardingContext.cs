using System;
using System.Collections.Generic;
using Features.Tutorial.Onboarding.UI;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic
{
    public sealed class OnboardingContext
    {
        private readonly OnboardingExplainerUI _explainerUI;
        private readonly OnboardingUIBlinker _uiBlinker;
        private readonly OnboardingMapBlinker _mapBlinker;
        private readonly IReadOnlyDictionary<int, string> _explainers;

        public OnboardingContext(
            OnboardingExplainerUI explainerUI,
            OnboardingUIBlinker uiBlinker,
            OnboardingMapBlinker mapBlinker,
            IReadOnlyDictionary<int, string> explainers)
        {
            _explainerUI = explainerUI;
            _uiBlinker = uiBlinker;
            _mapBlinker = mapBlinker;
            _explainers = explainers;
        }

        public void PostExplainer(int index, Action onNextClicked)
        {
            var message = _explainers.GetValueOrDefault(index, "Error");
            _explainerUI.Show(message, onNextClicked);
        }

        public void HideExplainer()
        {
            _explainerUI.Hide();
        }

        public void Blink(RectTransform targetTransform)
        {
            _mapBlinker.Hide();
            _uiBlinker.Show(targetTransform);
        }

        public void Blink(Vector2 targetPosition)
        {
            _uiBlinker.Hide();
            _mapBlinker.Show(targetPosition);
        }

        public void HideBlinker()
        {
            _mapBlinker.Hide();
            _uiBlinker.Hide();
        }
    }
}