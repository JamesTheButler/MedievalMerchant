using System.Collections.Generic;
using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using Features.Map;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Towns.Overlays
{
    public sealed class TownOverlayManager : InitializableBehavior
    {
        [SerializeField, Required]
        private TownOverlays townOverlays;

        private Dictionary<MapMode, ITownOverlayHandler> _overlayHandlers;

        private ITownOverlayHandler _activeHandler;
        private MapModeModel _mapModeModel;

        public override void Initialize()
        {
            var defaultHandler = new DefaultTownOverlayHandler();
            var multiHandler = new MultipleTownOverlayHandler();
            defaultHandler.SetUp(townOverlays);
            multiHandler.SetUp(townOverlays);

            _overlayHandlers = new Dictionary<MapMode, ITownOverlayHandler>
            {
                { MapMode.Default, defaultHandler },
                { MapMode.Town, multiHandler },
                { MapMode.Zone, defaultHandler },
            };

            _mapModeModel = GameplayContext.Instance.Model.MapModeModel;
            _mapModeModel.MapMode.Observe(OnMapModeChanged);
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _mapModeModel.MapMode.StopObserving(OnMapModeChanged);
        }

        public void OnCameraUpdated()
        {
            townOverlays.RefreshPositions();
        }

        public void OnTownClicked(Town town)
        {
            _activeHandler.OnTownClicked(town);
        }

        public void OnTownHovered(Town town)
        {
            _activeHandler.OnTownHovered(town);
        }

        public void OnTownUnhovered(Town town)
        {
            _activeHandler.OnTownUnhovered(town);
        }

        public void OnAnythingClicked()
        {
            _activeHandler.OnAnythingClicked();
        }

        private void OnMapModeChanged(MapMode mapMode)
        {
            _activeHandler?.Disable();
            _activeHandler = _overlayHandlers[mapMode];
            _activeHandler.Enable();
        }
    }
}