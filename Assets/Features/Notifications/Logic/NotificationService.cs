using System;
using Common.Infrastructure;
using UnityEngine;

namespace Features.Notifications.Logic
{
    public sealed class NotificationService : IService
    {
        public event Action<Notification> NotificationPosted;

        public void Initialize() { }
        public void CleanUp() { }

        public void PostNotification(Notification notification)
        {
            NotificationPosted?.Invoke(notification);
        }

        public void PingNotification(Notification notification)
        {
            Debug.LogError($"Pinging {notification.GetType().Name} not implemented yet.");
        }
    }
}