using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using Features.Ticking.Logic;

namespace Features.Ticking.UI
{
    public sealed class GamePausedIndicator : InitializableBehavior
    {
        private GameSpeedModel _gameSpeedModel;

        public override void Initialize()
        {
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _gameSpeedModel.IsPaused.Observe(OnPausedChanged);
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _gameSpeedModel.IsPaused.StopObserving(OnPausedChanged);
        }

        private void OnPausedChanged(bool isPaused)
        {
            gameObject.SetActive(isPaused);
        }
    }
}