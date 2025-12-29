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

        private void Start()
        {
            _notificationService = GameplayContext.Instance.Services.NotificationService;
            _notificationService.NotificationPosted += OnNotificationPosted;
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;

            panel.Hide();

            panel.Opened += OnPanelOpened;
            panel.Closed += OnPanelClosed;
        }

        public void ClosePanel()
        {
            panel.Hide();
        }

        private void OnNotificationPosted(Notification notification)
        {
           //if (notification.Severity != Severity.Major)
           //    return;

            panel.Show(notification);
        }

        private void OnPanelClosed()
        {
            _gameSpeedModel.Resume();
        }

        private void OnPanelOpened()
        {
            _gameSpeedModel.Pause();
        }
    }
}