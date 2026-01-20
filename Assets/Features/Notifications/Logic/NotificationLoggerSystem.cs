using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using UnityEngine;

namespace Features.Notifications.Logic
{
    public sealed class NotificationLoggerSystem : ISystem
    {
        private NotificationService _notificationService;
        private DateModel _gameDateModel;

        public void Initialize()
        {
            _gameDateModel = GameplayContext.Instance.Model.DateModel;
            _notificationService = GameplayContext.Instance.Services.NotificationService;
            _notificationService.NotificationPosted += LogNotification;
        }

        public void CleanUp()
        {
            _notificationService.NotificationPosted -= LogNotification;
        }

        private void LogNotification(Notification notif)
        {
            Debug.Log($"({_gameDateModel.GameDate.Value.ToDisplayString()}) {notif.Severity} Notification {notif.Title}: {notif.Description}");
        }
    }
}