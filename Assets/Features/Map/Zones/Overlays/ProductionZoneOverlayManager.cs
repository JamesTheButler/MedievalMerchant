using System.Collections.Generic;
using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using Features.Map.Modes;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Zones.Overlays
{
    public sealed class ProductionZoneOverlayManager : InitializableBehavior
    {
        [SerializeField, Required]
        private ProductionZoneOverlays productionZoneOverlays;

        private Dictionary<MapMode, IProductionZoneOverlayHandler> _overlayHandlers;

        private IProductionZoneOverlayHandler _activeHandler;
        private GameplayModel _gameplayModel;
        private MapModeModel _mapModeModel;

        public override void Initialize()
        {
            _mapModeModel = GameplayContext.Instance.Model.MapModeModel;
            _gameplayModel = GameplayContext.Instance.Model;

            var defaultHandler = new DefaultProductionZoneOverlayHandler();
            var multiHandler = new MultipleProductionZoneOverlayHandler();
            defaultHandler.SetUp(productionZoneOverlays);
            multiHandler.SetUp(productionZoneOverlays);

            _overlayHandlers = new Dictionary<MapMode, IProductionZoneOverlayHandler>
            {
                { MapMode.Default, defaultHandler },
                { MapMode.Town, defaultHandler },
                { MapMode.Zone, multiHandler },
            };
            SetUpPointerEvents();

            _mapModeModel.MapMode.Observe(OnMapModeChanged);
        }

        private void SetUpPointerEvents()
        {
            foreach (var productionZone in _gameplayModel.ProductionZones)
            {
                productionZone.Clicked += () => OnProductionZoneClicked(productionZone);
                productionZone.Hovered += () => OnProductionZoneHovered(productionZone);
                productionZone.Unhovered += () => OnProductionZoneUnhovered(productionZone);
            }
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _mapModeModel.MapMode.StopObserving(OnMapModeChanged);
        }

        public void OnCameraUpdated()
        {
            productionZoneOverlays.RefreshPositions();
        }

        private void OnProductionZoneClicked(ProductionZone productionZone)
        {
            _activeHandler.OnProductionZoneClicked(productionZone);
        }

        private void OnProductionZoneHovered(ProductionZone productionZone)
        {
            _activeHandler.OnProductionZoneHovered(productionZone);
        }

        private void OnProductionZoneUnhovered(ProductionZone productionZone)
        {
            _activeHandler.OnProductionZoneUnhovered(productionZone);
        }

        private void OnMapModeChanged(MapMode mapMode)
        {
            _activeHandler?.Disable();
            _activeHandler = _overlayHandlers[mapMode];
            _activeHandler.Enable();
        }
    }
}