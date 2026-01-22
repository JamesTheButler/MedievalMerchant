namespace Features.Towns.Overlays
{
    public interface ITownOverlayHandler
    {
        void SetUp(TownOverlays overlays);
        void Enable();
        void Disable();
        void OnTownClicked(Town town);
        void OnTownHovered(Town town);
        void OnTownUnhovered(Town town);
        void OnAnythingClicked();
    }
}