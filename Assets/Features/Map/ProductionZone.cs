using System;
using System.Collections.Generic;
using Common;
using Common.Types;
using Features.Towns.Production.Config;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

namespace Features.Map
{
    public sealed class ProductionZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action Clicked, Hovered, Unhovered;

        [field: SerializeField]
        public List<Good> AvailableGoods { get; private set; }

        [SerializeField, Required]
        private ProductionZoneConfig config;

        [SerializeField, Required]
        private GameObject origin;

        [field: SerializeField]
        public Region Region { get; private set; }

        public Vector3[] Points => _spriteController.spline.GetPoints();
        public Vector3 Position => transform.position;

        private SpriteShapeRenderer _spriteRenderer;
        private SpriteShapeController _spriteController;

        public Vector2 Center { get; private set; }

        private void Awake()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteShapeRenderer>();
            _spriteController = gameObject.GetComponent<SpriteShapeController>();
            Center = origin.transform.position;
            SetColor(config.DefaultColor);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Hovered?.Invoke();
            SetColor(config.SelectedColor);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Unhovered?.Invoke();
            SetColor(config.DefaultColor);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Clicked?.Invoke();
        }

        private void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }
    }
}