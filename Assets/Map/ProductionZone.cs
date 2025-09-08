using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

namespace Map
{
    public sealed class ProductionZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [field: SerializeField]
        public List<Good> AvailableGoods { get; set; }

        private SpriteShapeRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteShapeRenderer>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _spriteRenderer.color = Color.red.WithAlpha(0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _spriteRenderer.color = Color.white.WithAlpha(0.5f);
        }
    }
}