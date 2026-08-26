using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Overlays
{
    public sealed class OverlayIcon : MonoBehaviour
    {
        [SerializeField, Required]
        private SpriteRenderer spriteRenderer;

        public void SetUp(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}