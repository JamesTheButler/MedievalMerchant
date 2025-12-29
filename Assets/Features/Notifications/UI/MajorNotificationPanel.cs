using System;
using Common.UI.Utility;
using Features.Notifications.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Notifications.UI
{
    public sealed class MajorNotificationPanel : MonoBehaviour
    {
        public event Action Opened, Closed;

        [SerializeField]
        private TMP_Text titleText, descriptionText;

        [SerializeField]
        private Image icon;

        [SerializeField]
        private Button closeButton;

        public void Show(Notification notification)
        {
            var style = notification.Type switch
            {
                NotificationType.Info => Style.Default,
                NotificationType.Good => Style.Good,
                NotificationType.Bad => Style.Bad,
                _ => Style.Default
            };

            titleText.text = notification.Title.WithStyle(style);
            descriptionText.text = notification.Description;
            icon.gameObject.SetActive(notification.Icon != null);
            icon.sprite = notification.Icon;
            closeButton.onClick.AddListener(Hide);
            Opened?.Invoke();
        }

        public void Hide()
        {
            Closed?.Invoke();
        }
    }
}