using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Features.Ticking.Logic;
using UnityEngine;

namespace Features.Map.Overlays
{
    public sealed class GameSpeedAnimationHandler
    {
        private GameSpeedModel _gameSpeedModel;
        private Animation _animation;

        private float _animationSpeed;

        public void Initialize(Animation animation)
        {
            _animation = animation;
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _gameSpeedModel.IsPaused.Observe(OnGamePaused);
            _gameSpeedModel.GameSpeed.Observe(OnGameSpeedChanged);
        }

        public void CleanUp()
        {
            _gameSpeedModel.IsPaused.StopObserving(OnGamePaused);
            _gameSpeedModel.GameSpeed.StopObserving(OnGameSpeedChanged);
        }

        private void OnGameSpeedChanged(GameSpeed speed)
        {
            _animationSpeed = speed switch
            {
                GameSpeed.Normal => 1f,
                GameSpeed.Fast => 2f,
                _ => 1f
            };

            foreach (AnimationState state in _animation)
            {
                state.speed = _animationSpeed;
            }
        }

        private void OnGamePaused(bool isPaused)
        {
            var newSpeed = isPaused ? 0f : _animationSpeed;
            foreach (AnimationState state in _animation)
            {
                state.speed = newSpeed;
            }
        }
    }
}