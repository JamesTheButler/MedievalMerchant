using Common.Infrastructure.Gameplay;

namespace Features.Towns.Overlays
{
    public sealed class MultipleTownOverlayHandler : ITownOverlayHandler
    {
        private TownOverlays _townOverlays;
        private GameplayModel _gameplayModel;

        public void SetUp(TownOverlays overlays)
        {
            _gameplayModel = GameplayContext.Instance.Model;
            _townOverlays = overlays;
        }

        public void Enable()
        {
            foreach (var town in _gameplayModel.Towns.Values)
            {
                _townOverlays.Show(town);
            }
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