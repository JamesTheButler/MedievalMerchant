using Common.UI;
using UnityEngine;

namespace Common
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

        public static void DrawRect(Rect rect, Color? color = null, float padding = 0)
        {
            if (color != null)
            {
                Gizmos.color = color.Value;
            }

            Gizmos.DrawWireCube(
                rect.center + Vector2.one * padding,
                rect.size - Vector2.one * 2 * padding);
        }

        public static void DrawRect(RectTransform rectTransform, Color? color = null, float padding = 0)
        {
            if (!rectTransform)
                return;
            DrawRect(rectTransform.GetWorldRect(), color, padding);
        }
    }
}