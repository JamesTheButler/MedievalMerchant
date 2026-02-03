using Common.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace Features.Tutorial.Onboarding.UI
{
    public sealed class OnboardingMapBlinker : MonoBehaviour
    {
        [SerializeField, Required]
        private Sprite lmbSprite, rmbSprite;

        [SerializeField, Required]
        private SpriteRenderer mouseSpriteRenderer;

        public void Show(Vector2 worldLocation, MouseButton mouseButton)
        {
            gameObject.transform.localPosition = worldLocation.FromXY(gameObject.transform.localPosition.z);
            gameObject.SetActive(true);

            switch (mouseButton)
            {
                case MouseButton.Left:
                    mouseSpriteRenderer.sprite = lmbSprite;
                    mouseSpriteRenderer.gameObject.SetActive(true);
                    break;
                case MouseButton.Right:
                    mouseSpriteRenderer.sprite = rmbSprite;
                    mouseSpriteRenderer.gameObject.SetActive(true);
                    break;
                default:
                    mouseSpriteRenderer.gameObject.SetActive(false);
                    break;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}