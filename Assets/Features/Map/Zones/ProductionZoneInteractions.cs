using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Map.Zones
{
    public sealed class ProductionZoneInteractions : MonoBehaviour
    {
        [field: SerializeField]
        public UnityEvent<ProductionZone> ZoneHovered { get; private set; }

        [field: SerializeField]
        public UnityEvent<ProductionZone> ZoneSelected { get; private set; }

        private readonly Dictionary<ProductionZone, Action> _clickHandlers = new();
        private readonly Dictionary<ProductionZone, Action> _hoverHandlers = new();
        private readonly Dictionary<ProductionZone, Action> _unhoverHandlers = new();

        public void Initialize(ProductionZone[] productionZones)
        {
            foreach (var productionZone in productionZones)
            {
                _clickHandlers[productionZone] = OnClick;
                _hoverHandlers[productionZone] = OnHover;
                _unhoverHandlers[productionZone] = OnUnhover;

                productionZone.Clicked += OnClick;
                productionZone.Hovered += OnHover;
                productionZone.Unhovered += OnUnhover;
                continue;

                void OnUnhover() => ZoneHovered?.Invoke(null);
                void OnHover() => ZoneHovered?.Invoke(productionZone);
                void OnClick() => ZoneSelected?.Invoke(productionZone);
            }
        }

        public void CleanUp()
        {
            foreach (var (zone, action) in _clickHandlers)
            {
                zone.Clicked -= action;
            }

            foreach (var (zone, action) in _hoverHandlers)
            {
                zone.Hovered -= action;
            }

            foreach (var (zone, action) in _unhoverHandlers)
            {
                zone.Unhovered -= action;
            }

            _clickHandlers.Clear();
            _hoverHandlers.Clear();
            _unhoverHandlers.Clear();
        }

        public void DeselectActiveZone()
        {
            ZoneSelected?.Invoke(null);
        }
    }
}