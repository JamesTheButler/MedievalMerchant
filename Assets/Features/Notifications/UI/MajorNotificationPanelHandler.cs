using Common.Infrastructure;
using Features.Notifications.Logic;
using Features.Ticking.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Notifications.UI
{
    public sealed class MajorNotificationPanelHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private MajorNotificationPanel panel;

        private GameSpeedModel _gameSpeedModel;
        private NotificationService _notificationService;

        private Notification _currentNofNotification;

        private void Start()
        {
            _notificationService = GameplayContext.Instance.Services.NotificationService;
            _notificationService.NotificationPosted += OnNotificationPosted;
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;

            panel.Initialize();
            panel.Close();

            panel.Opened += OnPanelOpened;
            panel.Closed += OnPanelClosed;
            panel.Pinged += OnPingRequested;
        }

        private void OnPingRequested()
        {
            if (_currentNofNotification == null)
                return;

            _notificationService.PingNotification(_currentNofNotification);
        }

        public void ClosePanel()
        {
            panel.Close();
        }

        private void OnNotificationPosted(Notification notification)
        {
            if (notification.Severity != Severity.Major)
                return;

            _currentNofNotification = notification;
            panel.Setup(notification);
            panel.Open();
        }

        private void OnPanelClosed()
        {
            _gameSpeedModel.Resume();
            _currentNofNotification = null;
        }

        private void OnPanelOpened()
        {
            _gameSpeedModel.Pause();
        }
    }
}