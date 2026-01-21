using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Global;
using Common.Infrastructure.Observation;
using Features.Ticking.Logic;
using Features.Towns;
using Features.Trade;
using Features.Trade.Logic;

namespace Features.Audio.Sfx
{
    public sealed class GameSfxSystem : ISystem
    {
        private readonly Bindings _bindings = new();

        private SfxService _sfxService;

        public void Initialize()
        {
            _sfxService = GlobalContext.Instance.Services.SfxService;

            var playerModel = GameplayContext.Instance.Model.Player;
            var selection = GameplayContext.Instance.Selection;
            var gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            var navigationService = GameplayContext.Instance.Services.NavigationService;
            var tradeService = GameplayContext.Instance.Services.TradeService;

            _bindings.Track(
                playerModel.Location.CurrentTown.Observe(PlayerLocationChanged),
                selection.SelectedTown.Observe(SelectedTownChanged),
                navigationService.NavigationStarted.Observe(OnNavigationChanged),
                tradeService.TradeCompleted.Observe(OnTradeCompleted),
                //  LevelStarted // no hook exists for this, i think
                //  LevelConditions.LevelWon += ObsEvnt!
                //  LevelConditions.LevelLost += ObsEvnt!
                //  EventModel.EventAdded +=  ObsEvnt!
                gameSpeedModel.IsPaused.Observe(OnGamePaused),
                gameSpeedModel.GameSpeed.Observe(OnGameSpeedChanged)

                // UpgradeMissionStarted --- MissionModel.MissionAdded
                // i don't have a good way of detecting why a mission ended
                // UpgradeMissionCompleted  --- MissionModel.MissionRemoved
                // UpgradeMissionFailed --- MissionModel.MissionRemoved
                // TradeMissionCompleted --- MissionModel.MissionRemoved
                
                // ProducerBuilt
                // CartUpgraded
                // CartBought
                // CompanionUpgraded
            );
        }

        private void OnTradeCompleted(OngoingTrade info)
        {
            Play(GameSoundEffect.TradeCompleted);
        }

        public void CleanUp()
        {
            _bindings.UnbindAll();
        }

        private void OnNavigationChanged(Town town)
        {
            Play(GameSoundEffect.NavigationChanged);
        }

        private void OnGamePaused(bool isPaused)
        {
            var effect = isPaused ? GameSoundEffect.GamePaused : GameSoundEffect.GameResumed;
            Play(effect);
        }

        private void OnGameSpeedChanged(GameSpeed speed)
        {
            var effect = speed switch
            {
                GameSpeed.Normal => GameSoundEffect.GameSpeedChangedNormal,
                GameSpeed.Fast => GameSoundEffect.GameSpeedChangedFast,
                _ => GameSoundEffect.GameSpeedChangedNormal
            };
            Play(effect);
        }

        private void SelectedTownChanged(Town town)
        {
            if (town == null)
                return;
            Play(GameSoundEffect.TownSelected);
        }

        private void PlayerLocationChanged(Town town)
        {
            var effect = town == null ? GameSoundEffect.TownLeft : GameSoundEffect.TownEntered;
            Play(effect);
        }

        private void Play(GameSoundEffect soundEffect)
        {
            _sfxService.GameSoundEffect.Invoke(soundEffect);
        }
    }
}