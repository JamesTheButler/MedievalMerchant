using Common.Infrastructure;
using Common.UI.Elements;
using Features.Notifications.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Notifications.UI
{
    public sealed class MajorNotificationPanelHandler : InitializableBehavior
    {
        [SerializeField, Required]
        private MajorNotificationPanel panel;

        private NotificationService _notificationService;

        public override void Initialize()
        {
            _notificationService = GameplayContext.Instance.Services.NotificationService;
            _notificationService.NotificationPosted += OnNotificationPosted;

            panel.Close();
            panel.Pinged += OnPingRequested;
        }

        private void OnPingRequested(Notification notification)
        {
            _notificationService.PingNotification(notification);
        }

        private void OnNotificationPosted(Notification notification)
        {
            if (notification.Severity != Severity.Major)
                return;

            panel.Setup(notification);
            panel.Open();
        }
    }
}