using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Features.Ticking.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UI.Elements
{
    public sealed class DynamicPanelGamePauser : InitializableBehavior
    {
        [SerializeField, Required]
        private DynamicPanel dynamicPanel;

        private GameSpeedModel _gameSpeedModel;
        private bool _wasPausedBeforeOpened;

        public override void Initialize()
        {
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            dynamicPanel.Opened += OnPanelOpened;
            dynamicPanel.Closed += OnPanelClosed;
        }

        private void OnPanelOpened()
        {
            _wasPausedBeforeOpened = _gameSpeedModel.IsPaused.Value;
            if (!_wasPausedBeforeOpened)
            {
                _gameSpeedModel.Pause();
            }
        }

        private void OnPanelClosed()
        {
            if (!_wasPausedBeforeOpened)
            {
                _gameSpeedModel.Resume();
            }
        }
    }
}