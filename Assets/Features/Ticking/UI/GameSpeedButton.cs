using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Features.Ticking.Logic;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Ticking.UI
{
    public sealed class GameSpeedButton : MonoBehaviour
    {
        [SerializeField]
        private GameSpeed gameSpeed;

        [SerializeField]
        private Color defaultColor, highlightColor;

        private Button _button;

        private GameSpeedModel _gameSpeedModel;

        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(OnButtonClick);

            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _gameSpeedModel.GameSpeed.Observe(OnGameSpeedChanged);
            _gameSpeedModel.IsPaused.Observe(OnPausedChanged);
        }

        private void OnDestroy()
        {
            _gameSpeedModel.GameSpeed.StopObserving(OnGameSpeedChanged);
            _gameSpeedModel.IsPaused.StopObserving(OnPausedChanged);
        }

        private void OnGameSpeedChanged(GameSpeed speed)
        {
            UpdateButtonColor();
        }

        private void OnPausedChanged(bool isPaused)
        {
            UpdateButtonColor();
        }

        private void OnButtonClick()
        {
            _gameSpeedModel.Resume();
            _gameSpeedModel.GameSpeed.Value = gameSpeed;
        }

        private void UpdateButtonColor()
        {
            var isThisSelected = !_gameSpeedModel.IsPaused.Value && _gameSpeedModel.GameSpeed == gameSpeed;
            _button.image.color = isThisSelected ? highlightColor : defaultColor;
        }
    }
}