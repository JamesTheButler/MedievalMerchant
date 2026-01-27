using UnityEngine;

namespace Common.Utility
{
    public static class MyGizmos
    {
        public static void DrawX(Vector3 position, float size, Color? color = null)
        {
            if (color != null)
            {
                Gizmos.color = color.Value;
            }

            Gizmos.DrawLine(
                position + (Vector3.left + Vector3.up) * size,
                position + (Vector3.right + Vector3.down) * size);
            Gizmos.DrawLine(
                position + (Vector3.right + Vector3.up) * size,
                position + (Vector3.left + Vector3.down) * size);
        }

        public static void DrawRect(Rect rect, Color? color = null)
        {
            if (color != null)
            {
                Gizmos.color = color.Value;
            }

            Gizmos.DrawWireCube(rect.center.FromXY(), rect.size.FromXY(1f));
        }

        public static void DrawRectOnCanvas(Canvas canvas, Rect rect, Color color)
        {
            var canvasRectTransform = canvas.GetComponent<RectTransform>();
            if (canvasRectTransform == null)
            {
                Debug.LogError("Canvas.RectTransform is null!");
                return;
            }

            Gizmos.color = color;

            var bottomLeft = canvasRectTransform.TransformPoint(new Vector3(rect.xMin, rect.yMin, 0));
            var topLeft = canvasRectTransform.TransformPoint(new Vector3(rect.xMin, rect.yMax, 0));
            var topRight = canvasRectTransform.TransformPoint(new Vector3(rect.xMax, rect.yMax, 0));
            var bottomRight = canvasRectTransform.TransformPoint(new Vector3(rect.xMax, rect.yMin, 0));

            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
        }
    }
}