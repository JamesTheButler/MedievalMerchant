namespace Features.Towns.Overlays
{
    public sealed class DefaultTownOverlayHandler : ITownOverlayHandler
    {
        private TownOverlays _townOverlays;
        private Town _activeTown;

        public void SetUp(TownOverlays overlays)
        {
            _townOverlays = overlays;
        }

        public void Enable() { }

        public void Disable()
        {
            HideActive();
        }

        public void OnTownClicked(Town town) { }

        public void OnTownHovered(Town town)
        {
            HideActive();

            _townOverlays.Show(town);

            _activeTown = town;
        }

        public void OnTownUnhovered(Town town)
        {
            HideActive();
        }

        public void OnAnythingClicked() { }

        private void HideActive()
        {
            if (_activeTown == null)
                return;
            _townOverlays.Hide(_activeTown);
            _activeTown = null;
        }
    }
}