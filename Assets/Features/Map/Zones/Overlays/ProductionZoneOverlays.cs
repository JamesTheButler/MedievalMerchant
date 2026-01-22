using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Zones.Overlays
{
    public sealed class ProductionZoneOverlays : MonoBehaviour
    {
        [SerializeField, Required]
        private Canvas overlayCanvas;

        [SerializeField, Required]
        private ProductionZoneOverlay overlayPrefab;

        private readonly Dictionary<ProductionZone, ProductionZoneOverlay> _overlays = new();

        public void Show(ProductionZone productionZone)
        {
            if (!_overlays.TryGetValue(productionZone, out var overlay))
            {
                overlay = Instantiate(overlayPrefab, overlayCanvas.transform);
                overlay.SetUp(productionZone);
                _overlays[productionZone] = overlay;
            }

            overlay.Open();
        }

        public void Hide(ProductionZone productionZone)
        {
            if (_overlays.TryGetValue(productionZone, out var overlay))
            {
                overlay.Close();
            }
        }

        public void HideAll()
        {
            foreach (var overlay in _overlays.Values)
            {
                overlay.Close();
            }
        }

        public void RefreshPositions()
        {
            foreach (var overlay in _overlays.Values)
            {
                overlay.RefreshPosition();
            }
        }
    }
}