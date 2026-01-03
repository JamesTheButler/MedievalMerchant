using Common.Infrastructure;
using Common.Types;
using UnityEngine;

namespace Features.Notifications.Logic
{
    public sealed class NotificationLoggerSystem : ISystem
    {
        private NotificationService _notificationService;
        private Date _gameDate;

        public void Initialize()
        {
            _gameDate = GameplayContext.Instance.Model.Date;
            _notificationService = GameplayContext.Instance.Services.NotificationService;
            _notificationService.NotificationPosted += LogNotification;
        }

        public void CleanUp()
        {
            _notificationService.NotificationPosted -= LogNotification;
        }

        private void LogNotification(Notification notif)
        {
            Debug.Log($"({_gameDate}){notif.Severity} Notification {notif.Title}: {notif.Description}");
        }
    }
}