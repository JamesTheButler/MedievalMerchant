using NaughtyAttributes;
using UnityEngine;

namespace Features.Tutorial.Onboarding.UI
{
    public sealed class OnboardingUIBlinker : MonoBehaviour
    {
        [SerializeField, Required]
        private RectTransform rectTransform;

        public void Show(RectTransform targetTransform)
        {
            var targetCenter = targetTransform.rect.center;
            rectTransform.anchoredPosition = targetCenter;
            rectTransform.sizeDelta = targetTransform.rect.size;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}