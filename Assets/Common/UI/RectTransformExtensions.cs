using UnityEngine;

namespace Common.UI
{
    public static class RectTransformExtensions
    {
        /// <summary>
        /// order: 0=bottom-left, 1=top-left, 2=top-right, 3=bottom-right
        /// </summary>
        public static Vector3[] GetCorners(this RectTransform rectTransform)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return corners;
        }

        public static Rect GetWorldRect(this RectTransform rectTransform)
        {
            var corners = rectTransform.GetCorners();
            var bottomLeft = corners[0];
            return new Rect(
                bottomLeft.x,
                bottomLeft.y,
                rectTransform.rect.width,
                rectTransform.rect.height
            );
        }

        /// <summary>
        /// Sets the world position of the transform while clamping it within the screen bounds of the specified canvas.
        /// </summary>
        public static void SetCanvasClampedPosition(
            this Transform transform,
            Vector3 worldPosition,
            Canvas canvas,
            float padding = 0f)
        {
            var rectTransform = transform as RectTransform;

            if (rectTransform == null || canvas == null)
                return;

            // Convert desired world position to screen space
            var screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, worldPosition);

            // Calculate tooltip size in screen space (considering scale factor)
            var tooltipSize = rectTransform.sizeDelta * canvas.scaleFactor;

            var halfWidth = tooltipSize.x / 2;
            var halfHeight = tooltipSize.y / 2;

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // Clamp X within screen bounds
            screenPoint.x = Mathf.Clamp(screenPoint.x, padding + halfWidth, screenWidth - padding - halfWidth);
            screenPoint.y = Mathf.Clamp(screenPoint.y, padding + halfHeight, screenHeight - padding - halfHeight);

            var inScreen = RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.transform as RectTransform,
                screenPoint,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out var clampedWorldPos);
            // Convert clamped screen point back to world space
            if (inScreen)
            {
                transform.position = clampedWorldPos;
            }
        }
    }
}