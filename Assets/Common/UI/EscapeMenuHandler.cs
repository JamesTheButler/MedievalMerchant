using Common.Infrastructure;
using Features.Ticking.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UI
{
    public sealed class EscapeMenuHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private EscapeMenu escapeMenu;

        private GameSpeedModel _gameSpeedModel;

        private void Start()
        {
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;

            escapeMenu.Initialize();
            escapeMenu.Opened += OnMenuOpened;
            escapeMenu.Closed += OnMenuClosed;
        }

        public void ToggleMenu() => escapeMenu.Toggle();
        private void OnMenuOpened() => _gameSpeedModel.Pause();
        private void OnMenuClosed() => _gameSpeedModel.Resume();
    }
}