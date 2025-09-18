using UnityEngine;

namespace Common
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
            return  RectTransformUtility.WorldToScreenPoint(
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                worldTopCenter
            );
        }
    }
}