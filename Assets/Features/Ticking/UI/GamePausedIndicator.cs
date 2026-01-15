using Common.Infrastructure.Gameplay;
using Features.Ticking.Logic;
using UnityEngine;

namespace Features.Ticking.UI
{
    public sealed class GamePausedIndicator : MonoBehaviour
    {
        private GameSpeedModel _gameSpeedModel;

        private void Awake()
        {
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _gameSpeedModel.IsPaused.Observe(OnPausedChanged);
        }

        private void OnDestroy()
        {
            _gameSpeedModel.IsPaused.StopObserving(OnPausedChanged);
        }

        private void OnPausedChanged(bool isPaused)
        {
            gameObject.SetActive(isPaused);
        }
    }
}