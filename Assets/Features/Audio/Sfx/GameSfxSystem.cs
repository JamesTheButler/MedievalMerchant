using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Global;
using Common.Infrastructure.Observation;
using Features.Map.Pathfinding;
using Features.Ticking.Logic;
using Features.Towns;
using Features.Towns.Production.Logic;
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

            var model = GameplayContext.Instance.Model;

            var playerModel = model.Player;
            var gameSpeedModel = model.GameSpeed;
            var levelConditions = model.Conditions;

            var selection = GameplayContext.Instance.Selection;
            var navigationService = GameplayContext.Instance.Services.NavigationService;
            var tradeService = GameplayContext.Instance.Services.TradeService;

            _bindings.Track(
                playerModel.Location.MapLocation.Observe(PlayerLocationChanged),
                selection.SelectedTown.Observe(SelectedTownChanged),
                navigationService.NavigationStarted.Observe(OnNavigationChanged),
                tradeService.TradeCompleted.Observe(OnTradeCompleted),
                //  LevelStarted // no hook exists for this, i think
                levelConditions.LevelWon.Observe(() => Play(GameSoundEffect.LevelWon)),
                levelConditions.LevelLost.Observe(_ => Play(GameSoundEffect.LevelLost)),
                //  EventModel.EventAdded +=  ObsEvnt!
                gameSpeedModel.IsPaused.Observe(OnGamePaused),
                gameSpeedModel.GameSpeed.Observe(OnGameSpeedChanged)

                // UpgradeMissionStarted --- MissionModel.MissionAdded
                // i don't have a good way of detecting why a mission ended
                // UpgradeMissionCompleted  --- MissionModel.MissionRemoved
                // UpgradeMissionFailed --- MissionModel.MissionRemoved
                // TradeMissionCompleted --- MissionModel.MissionRemoved

                // CartUpgraded
                // CartBought
                // CompanionUpgraded
            );

            foreach (var town in model.Towns.Values)
            {
                _bindings.Track(
                    town.ProductionManager.ProductionAdded.Observe(OnProducerAdded)
                );
            }
        }

        private void OnProducerAdded(Producer producer)
        {
            Play(GameSoundEffect.ProducerBuilt);
        }

        private void OnTradeCompleted(CompletedTrade info)
        {
            Play(GameSoundEffect.TradeCompleted);
        }

        public void CleanUp()
        {
            _bindings.Unbind();
        }

        private void OnNavigationChanged(IMapLocation location)
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

        private void PlayerLocationChanged(IMapLocation location)
        {
            var effect = location == null ? GameSoundEffect.TownLeft : GameSoundEffect.TownEntered;
            Play(effect);
        }

        private void Play(GameSoundEffect soundEffect)
        {
            _sfxService.GameSoundEffect.Invoke(soundEffect);
        }
    }
}