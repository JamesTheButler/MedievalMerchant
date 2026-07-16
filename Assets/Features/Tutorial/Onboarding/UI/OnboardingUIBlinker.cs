using Common.UI.Utility;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

namespace Features.Tutorial.Onboarding.UI
{
    public sealed class OnboardingUIBlinker : MonoBehaviour
    {
        [SerializeField]
        private float padding = 4f;

        [SerializeField, Required]
        private RectTransform rectTransform;

        [SerializeField, Required]
        private Image mouseButtonImage;

        [SerializeField, Required]
        private Sprite lmbSprite, rmbSprite;

        public void Show(MonoBehaviour target, MouseButton mouseButton)
        {
            var targetTransform = target?.GetComponent<RectTransform>();
            Show(targetTransform, mouseButton);
        }

        public void Show(RectTransform targetTransform, MouseButton mouseButton)
        {
            if (!targetTransform)
            {
                gameObject.SetActive(false);
                return;
            }

            SetBlinkerRectTransform(targetTransform);
            gameObject.SetActive(true);

            switch (mouseButton)
            {
                case MouseButton.Left:
                    mouseButtonImage.sprite = lmbSprite;
                    mouseButtonImage.gameObject.SetActive(true);
                    break;
                case MouseButton.Right:
                    mouseButtonImage.sprite = rmbSprite;
                    mouseButtonImage.gameObject.SetActive(true);
                    break;
                default:
                    mouseButtonImage.gameObject.SetActive(false);
                    break;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void SetBlinkerRectTransform(RectTransform target)
        {
            var worldCorners = target.GetWorldCorners();
            var parent = (RectTransform)rectTransform.parent;

            // anchoredPosition is measured from the parent's rect.min in the parent's local
            // space, not from world space directly - these only coincide when the Canvas
            // Scaler's scale factor is 1, i.e. exactly at the reference resolution.
            var bottomLeft = (Vector2)parent.InverseTransformPoint(worldCorners[0]) - parent.rect.min;
            var topRight = (Vector2)parent.InverseTransformPoint(worldCorners[2]) - parent.rect.min;
            var size = topRight - bottomLeft + new Vector2(2 * padding, 2 * padding);

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.zero;
            rectTransform.pivot = new Vector2(.5f, .5f);
            rectTransform.anchoredPosition = bottomLeft + (topRight - bottomLeft) * .5f;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
        }
    }
}