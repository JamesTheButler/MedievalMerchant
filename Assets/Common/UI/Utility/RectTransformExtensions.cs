using System.Linq;
using UnityEngine;

namespace Common.UI.Utility
{
    public static class RectTransformExtensions
    {
        /// <summary>
        /// order: 0=bottom-left, 1=top-left, 2=top-right, 3=bottom-right
        /// </summary>
        public static Vector3[] GetWorldCorners(this RectTransform rectTransform)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return corners;
        }

        public static Vector3 GetCenter(this RectTransform rectTransform)
        {
            var corners = rectTransform.GetWorldCorners();
            var center = corners.Aggregate(Vector3.zero, (curr, next) => curr + next / corners.Length);

            return center;
        }

        public static Rect GetWorldRect(this RectTransform rectTransform)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            var bottomLeft = corners[0];
            return new Rect(
                bottomLeft.x,
                bottomLeft.y,
                rectTransform.rect.width,
                rectTransform.rect.height
            );
        }
    }
}