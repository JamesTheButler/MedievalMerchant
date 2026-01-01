using System;
using Common.Infrastructure;

namespace Features.Notifications.Logic
{
    public sealed class NotificationService : IService
    {
        public event Action<Notification> NotificationPosted;
        public event Action<Notification> NotificationPinged;

        public void Initialize() { }
        public void CleanUp() { }

        public void PostNotification(Notification notification)
        {
            NotificationPosted?.Invoke(notification);
        }

        public void PingNotification(Notification notification)
        {
            NotificationPinged?.Invoke(notification);
        }
    }
}