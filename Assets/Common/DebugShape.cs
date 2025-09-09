using System;
using NaughtyAttributes;
using UnityEngine;

namespace Common
{
    public sealed class DebugShape : MonoBehaviour
    {
        [SerializeField]
        private DebugShapeType shape;

        [SerializeField, ShowIf(nameof(shape), DebugShapeType.Sphere)]
        private float radius = 1f;

        [SerializeField, ShowIf(nameof(shape), DebugShapeType.Square)]
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
                        Gizmos.DrawSphere(position, radius);
                    }
                    else
                    {
                        Gizmos.DrawWireSphere(position, radius);
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
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}