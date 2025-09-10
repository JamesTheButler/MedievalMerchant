using System.Collections.Generic;
using Data;
using Data.Configuration;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

namespace Map
{
    public sealed class ProductionZone : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
    {
        [field: SerializeField]
        public List<Good> AvailableGoods { get; private set; }

        [SerializeField, Required]
        private ProductionZoneConfig config;

        [SerializeField, Required]
        private GameObject origin;

        private SpriteShapeRenderer _spriteRenderer;
        private SpriteShapeController _spriteController;

        public Vector2 Center { get; private set; }

        private void Start()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteShapeRenderer>();

            // TODO: use to force points of 2d polygon collider
            _spriteController = gameObject.GetComponent<SpriteShapeController>();
            Center = origin.transform.position;

            OnPointerExit(null);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExit();
        }

        private void PointerExit()
        {
            FindFirstObjectByType<ProductionZoneManager>()?.OnZoneSelected.Invoke(null);
            _spriteRenderer.color = config.DefaultColor;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (IsPointerOverTown(eventData.position))
            {
                PointerExit();
            }
            else
            {
                PointerEnter();
            }
        }

        private bool IsPointerOverTown(Vector2 eventDataPosition)
        {
            // TODO: towns should be clickable through production zones
            //var world = Camera.main!.ScreenToWorldPoint(eventDataPosition);
            //return Physics2D.OverlapPoint(world, townLayerMask) != null;
            return false;
        }

        private void PointerEnter()
        {
            FindFirstObjectByType<ProductionZoneManager>()?.OnZoneSelected.Invoke(this);
            _spriteRenderer.color = config.SelectedColor;
        }
    }
}