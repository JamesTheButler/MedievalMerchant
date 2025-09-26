using System;
using UnityEngine;

namespace Common
{
    public sealed class DebugShape : MonoBehaviour
    {
        [SerializeField]
        private DebugShapeType shape;

        [SerializeField]
        private float size = 1f;

        [SerializeField]
        private bool onSelectionOnly;

        [SerializeField]
        private bool fill;

        [SerializeField]
        private Color color = Color.green;

        private void OnDrawGizmosSelected()
        {
            if (!onSelectionOnly) return;
            Render();
        }

        private void OnDrawGizmos()
        {
            if (onSelectionOnly) return;
            Render();
        }

        private void Render()
        {
            Gizmos.color = color;
            var position = transform.position;
            switch (shape)
            {
                case DebugShapeType.Sphere:
                    if (fill)
                    {
                        Gizmos.DrawSphere(position, size);
                    }
                    else
                    {
                        Gizmos.DrawWireSphere(position, size);
                    }

                    break;

                case DebugShapeType.Square:
                    var cubeSize = new Vector3(size, size, size);
                    if (fill)
                    {
                        Gizmos.DrawCube(position, cubeSize);
                    }
                    else
                    {
                        Gizmos.DrawWireCube(position, cubeSize);
                    }

                    break;

                case DebugShapeType.X:
                    Gizmos.DrawLine(
                        position + (Vector3.left + Vector3.up) * size,
                        position + (Vector3.right + Vector3.down) * size);
                    Gizmos.DrawLine(
                        position + (Vector3.right + Vector3.up) * size,
                        position + (Vector3.left + Vector3.down) * size);
                    break;

                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}