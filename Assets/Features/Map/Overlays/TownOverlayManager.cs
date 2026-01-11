using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.UI.Elements;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Overlays
{
    public sealed class TownOverlayManager : InitializableBehavior
    {
        [SerializeField, Required]
        private GameObject townOverlayParent;

        [SerializeField, Required]
        private TownOverlay townOverlayPrefab;

        private readonly List<TownOverlay> _overlays = new();

        public override void Initialize()
        {
            foreach (var town in GameplayContext.Instance.Model.Towns.Values)
            {
                var overlay = Instantiate(townOverlayPrefab, townOverlayParent.transform);
                overlay.Bind(town);
                _overlays.Add(overlay);
            }
        }

        public override void CleanUp()
        {
            foreach (var overlay in _overlays)
            {
                overlay.Unbind();
            }
        }
    }
}