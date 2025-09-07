using System;
using System.Collections.Generic;
using Data;
using NaughtyAttributes;
using UnityEngine;

namespace Map.Overlays
{
    public sealed class TownOverlayManager : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject townOverlayParent;

        [SerializeField, Required]
        private GameObject townOverlayPrefab;

        private readonly Lazy<Model> _model = new(() => Model.Instance);

        private readonly List<TownOverlay> _overlays = new();

        public void Initialize()
        {
            var model = _model.Value;
            foreach (var town in model.Towns.Values)
            {
                var overlayObject = Instantiate(townOverlayPrefab, townOverlayParent.transform);
                var overlay = overlayObject.GetComponent<TownOverlay>();
                overlay.Bind(town);
                _overlays.Add(overlay);
            }
        }

        private void OnDestroy()
        {
            foreach (var overlay in _overlays)
            {
                overlay.Unbind();
            }
        }
    }
}