using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Features.Notifications.Logic;
using UnityEngine;

namespace Features.Notifications.UI
{
    public sealed class NotificationPanel : MonoBehaviour
    {
        [SerializeField]
        private int maxAmount = 3;

        [SerializeField]
        private GameObject notificationPrefab;

        [SerializeField]
        private RectTransform notificationRect;

        private readonly List<NotificationItem> _notificationItems = new();

        private NotificationService _notificationService;

        private void Start()
        {
            _notificationService = GameplayContext.Instance.Services.NotificationService;
            _notificationService.NotificationPosted += OnNotificationReceived;
        }

        private void OnDestroy()
        {
            _notificationService.NotificationPosted -= OnNotificationReceived;
        }

        private void OnNotificationReceived(Notification notification)
        {
            if (_notificationItems.Count >= maxAmount)
            {
                var item = _notificationItems.First();
                _notificationItems.Remove(item);
                item.Close();
            }

            var notificationItem = Instantiate(notificationPrefab, notificationRect).GetComponent<NotificationItem>();
            notificationItem.SetUp(
                notification,
                () => _notificationService.PingNotification(notification),
                () => _notificationItems.Remove(notificationItem));

            _notificationItems.Add(notificationItem);
        }
    }
}