using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Towns.Overlays
{
    public sealed class TownOverlays : MonoBehaviour
    {
        [SerializeField, Required]
        private Canvas overlayCanvas;

        [SerializeField, Required]
        private TownOverlay overlayPrefab;

        private readonly Dictionary<Town, TownOverlay> _overlays = new();

        public void Show(Town town)
        {
            if (!_overlays.TryGetValue(town, out var overlay))
            {
                overlay = Instantiate(overlayPrefab, overlayCanvas.transform);
                overlay.SetUp(town);
                _overlays[town] = overlay;
            }

            overlay.Open();
        }

        public void Hide(Town town)
        {
            if (_overlays.TryGetValue(town, out var overlay))
            {
                overlay.Close();
            }
        }

        public void ShowAll()
        {
            foreach (var overlay in _overlays.Values)
            {
                overlay.Open();
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