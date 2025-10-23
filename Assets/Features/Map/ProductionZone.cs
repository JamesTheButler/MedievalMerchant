using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Types;
using Features.Towns.Production.Config;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

namespace Features.Map
{
    public sealed class ProductionZone : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
    {
        [field: SerializeField]
        public List<Good> AvailableGoods { get; private set; }

        [SerializeField, Required]
        private ProductionZoneConfig config;

        [SerializeField, Required]
        private GameObject origin;

        [field: SerializeField]
        public Region Region { get; private set; }

        private SpriteShapeRenderer _spriteRenderer;
        private SpriteShapeController _spriteController;

        public Vector2 Center { get; private set; }

        private void Awake()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteShapeRenderer>();
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
            PointerEnter();
        }

        private void PointerEnter()
        {
            FindFirstObjectByType<ProductionZoneManager>()?.OnZoneSelected.Invoke(this);
            _spriteRenderer.color = config.SelectedColor;
        }

        public bool IsAdjacentTo(Vector2Int position, float distanceThreshold)
        {
            var points = _spriteController.spline.GetPoints();
            var zoneOrigin = transform.position;
            return points.Any(zonePoint => Vector2.Distance(position, zonePoint + zoneOrigin) <= distanceThreshold);
        }
    }
}