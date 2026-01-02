using System;
using Common.Infrastructure;
using UnityEngine;

namespace Features.Ticking.Logic
{
    public sealed class GameSpeedInputHandler : MonoBehaviour
    {
        private readonly Lazy<GameSpeedModel> _gameSpeedModel = new(() => GameplayContext.Instance.Model.GameSpeed);

        public void PauseGame()
        {
            _gameSpeedModel.Value.Pause();
        }

        public void PlayGame()
        {
            _gameSpeedModel.Value.Resume();
            _gameSpeedModel.Value.GameSpeed.Value = GameSpeed.Normal;
        }

        public void PlayGameFast()
        {
            _gameSpeedModel.Value.Resume();
            _gameSpeedModel.Value.GameSpeed.Value = GameSpeed.Fast;
        }
    }
}