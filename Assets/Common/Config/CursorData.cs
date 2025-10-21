using System;
using NaughtyAttributes;
using UnityEngine;

namespace Common.Config
{
    [Serializable]
    public sealed record CursorData
    {
        [field: SerializeField, Required]
        public Texture2D Texture { get; private set; }

        [field: SerializeField]
        public Vector2 HotSpot { get; private set; }
    }
}