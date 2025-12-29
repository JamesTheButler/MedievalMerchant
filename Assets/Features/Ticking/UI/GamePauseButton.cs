using Common.Infrastructure;
using Features.Ticking.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Ticking.UI
{
    public sealed class GamePauseButton : MonoBehaviour
    {
        [SerializeField]
        private Color defaultColor, highlightColor;

        private Button _button;

        private GameSpeedModel _gameSpeedModel;

        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(OnButtonClick);
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _gameSpeedModel.IsPaused.Observe(OnPausedChanged);
        }

        private void OnDestroy()
        {
            _gameSpeedModel.IsPaused.StopObserving(OnPausedChanged);
        }

        private void OnPausedChanged(bool isPaused)
        {
            _button.image.color = isPaused ? highlightColor : defaultColor;
        }

        private void OnButtonClick()
        {
            _gameSpeedModel.Pause();
        }

    }
}