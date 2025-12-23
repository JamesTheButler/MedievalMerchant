using System;
using Common.Infrastructure;

namespace Features.Notifications
{
    public sealed class NotificationService : IService
    {
        public event Action<string> NotificationPosted;

        public void Initialize() { }
        public void CleanUp() { }

        public void PostNotification(string message)
        {
            NotificationPosted?.Invoke(message);
        }
    }
}