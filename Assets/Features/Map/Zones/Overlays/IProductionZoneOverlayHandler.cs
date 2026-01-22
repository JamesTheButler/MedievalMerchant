namespace Features.Map.Zones.Overlays
{
    public interface IProductionZoneOverlayHandler
    {
        void SetUp(ProductionZoneOverlays overlays);
        void Enable();
        void Disable();
        void OnProductionZoneClicked(ProductionZone productionZone);
        void OnProductionZoneHovered(ProductionZone productionZone);
        void OnProductionZoneUnhovered(ProductionZone productionZone);
        void OnAnythingClicked();
    }
}