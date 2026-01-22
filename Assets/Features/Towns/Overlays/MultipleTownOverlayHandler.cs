namespace Features.Towns.Overlays
{
    public sealed class MultipleTownOverlayHandler : ITownOverlayHandler
    {
        private TownOverlays _townOverlays;

        public void SetUp(TownOverlays overlays)
        {
            _townOverlays = overlays;
        }

        public void Enable()
        {
            _townOverlays.ShowAll();
        }

        public void Disable()
        {
            _townOverlays.HideAll();
        }

        public void OnTownClicked(Town town) { }

        public void OnTownHovered(Town town) { }

        public void OnTownUnhovered(Town town) { }

        public void OnAnythingClicked() { }
    }
}