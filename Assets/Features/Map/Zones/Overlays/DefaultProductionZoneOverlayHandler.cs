namespace Features.Map.Zones.Overlays
{
    public sealed class DefaultProductionZoneOverlayHandler : IProductionZoneOverlayHandler
    {
        private ProductionZoneOverlays _productionZoneOverlays;
        private ProductionZone _activeProductionZone;

        public void SetUp(ProductionZoneOverlays overlays)
        {
            _productionZoneOverlays = overlays;
        }

        public void Enable() { }

        public void Disable()
        {
            HideActive();
        }

        public void OnProductionZoneClicked(ProductionZone productionZone) { }

        public void OnProductionZoneHovered(ProductionZone productionZone)
        {
            HideActive();

            _productionZoneOverlays.Show(productionZone);

            _activeProductionZone = productionZone;
        }

        public void OnProductionZoneUnhovered(ProductionZone productionZone)
        {
            HideActive();
        }

        public void OnAnythingClicked() { }

        private void HideActive()
        {
            if (_activeProductionZone == null)
                return;

            _productionZoneOverlays.Hide(_activeProductionZone);
            _activeProductionZone = null;
        }
    }
}