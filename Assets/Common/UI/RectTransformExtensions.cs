using UnityEngine;

namespace Common.UI
{
    public static class RectTransformExtensions
    {
        public static Vector3 GetTopCenter(this RectTransform rectTransform)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            // order: 0=bottom-left, 1=top-left, 2=top-right, 3=bottom-right
            var worldTopCenter = (corners[1] + corners[2]) * 0.5f;

            var canvas = rectTransform.GetComponentInParent<Canvas>();
            return RectTransformUtility.WorldToScreenPoint(
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                worldTopCenter
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

            var i = RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.transform as RectTransform,
                screenPoint,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out var clampedWorldPos);
            // Convert clamped screen point back to world space
            if (i)
            {
                transform.position = clampedWorldPos;
            }
        }
    }
}