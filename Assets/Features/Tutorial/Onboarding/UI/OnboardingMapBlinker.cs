using Common.Utility;
using UnityEngine;

namespace Features.Tutorial.Onboarding.UI
{
    public sealed class OnboardingMapBlinker : MonoBehaviour
    {
        public void Show(Vector2 worldLocation)
        {
            gameObject.transform.localPosition = worldLocation.FromXY(gameObject.transform.localPosition.z);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}