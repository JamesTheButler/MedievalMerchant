using Common.Camera;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.UI;
using Common.UI.Elements.Panels;
using Features.Levels.Conditions.Model;
using Features.Levels.GameModifiers.Events;
using Features.Towns;
using Features.Towns.Missions;
using UnityEngine;

namespace Features.Notifications.Logic
{
    public sealed class NotificationPingSystem : ISystem
    {
        private CameraService _cameraService;
        private NotificationService _notificationService;
        private UIBridgeService _uiBridgeService;

        public void Initialize()
        {
            _cameraService = GameplayContext.Instance.Services.CameraService;
            _notificationService = GameplayContext.Instance.Services.NotificationService;
            _uiBridgeService = GameplayContext.Instance.Services.UIBridgeService;

            _notificationService.NotificationPinged += OnNotificationPinged;
        }

        public void CleanUp()
        {
            _notificationService.NotificationPinged -= OnNotificationPinged;
        }

        private void OnNotificationPinged(Notification notification)
        {
            switch (notification)
            {
                case EventExpiredNotification:
                    // nothing to do
                    break;
                case EventStartedNotification:
                    _uiBridgeService.OpenPanelFromBackend(UIPanel.LevelConditions);
                    break;
                case MissionFailedNotification missionFailedNotification:
                    PingTown(missionFailedNotification.Town);
                    break;
                case MissionStartedNotification missionStartedNotification:
                    PingTown(missionStartedNotification.Town);
                    break;
                case LossConditionNotification:
                    _uiBridgeService.OpenPanelFromBackend(UIPanel.WinLossConditions);
                    break;
                default:
                    Debug.LogError($"Unhandled notification ping {notification.GetType().Name}");
                    break;
            }
        }

        private void PingTown(Town town)
        {
            GameplayContext.Instance.Selection.Select(town);
            _cameraService.FocusCamera(town);
        }
    }
}