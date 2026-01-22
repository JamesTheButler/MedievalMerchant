using Common.Infrastructure.Gameplay;

namespace Features.Map.Zones.Overlays
{
    public sealed class MultipleProductionZoneOverlayHandler : IProductionZoneOverlayHandler
    {
        private ProductionZoneOverlays _productionZoneOverlays;
        private GameplayModel _gameplayModel;

        public void SetUp(ProductionZoneOverlays overlays)
        {
            _gameplayModel = GameplayContext.Instance.Model;
            _productionZoneOverlays = overlays;
        }

        public void Enable()
        {
            foreach (var productionZone in _gameplayModel.ProductionZones)
            {
                _productionZoneOverlays.Show(productionZone);
            }
        }

        public void Disable()
        {
            _productionZoneOverlays.HideAll();
        }

        public void OnProductionZoneClicked(ProductionZone productionZone) { }

        public void OnProductionZoneHovered(ProductionZone productionZone) { }

        public void OnProductionZoneUnhovered(ProductionZone productionZone) { }

        public void OnAnythingClicked() { }
    }
}