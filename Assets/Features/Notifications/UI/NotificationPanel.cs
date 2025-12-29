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
        private float popupLifetimeInSec = 5f;

        [SerializeField]
        private GameObject notificationPrefab;

        [SerializeField]
        private RectTransform notificationRect;

        private readonly List<MinorNotificationItem> _notificationItems = new();

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
            if (notification.Severity != Severity.Minor)
                return;
            
            if (_notificationItems.Count >= maxAmount)
            {
                var item = _notificationItems.First();
                _notificationItems.Remove(item);
                item.Close();
            }

            var notificationItem = Instantiate(notificationPrefab, notificationRect).GetComponent<MinorNotificationItem>();
            notificationItem.SetUp(
                notification,
                popupLifetimeInSec,
                () => _notificationService.PingNotification(notification),
                () => _notificationItems.Remove(notificationItem));

            _notificationItems.Add(notificationItem);
        }
    }
}