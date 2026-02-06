using System;
using Common.UI.Elements;
using Common.UI.Elements.Panels;
using Common.UI.Utility;
using Features.Notifications.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Notifications.UI
{
    public sealed class MajorNotificationPanel : DynamicPanel
    {
        public event Action<Notification> Pinged;

        [SerializeField]
        private TMP_Text titleText, descriptionText;

        [SerializeField]
        private Image icon;

        [SerializeField]
        private Button closeButton, pingButton;

        private Notification _notification;

        protected override void OnInitialize()
        {
            closeButton.onClick.AddListener(Close);
            pingButton.onClick.AddListener(PingNotification);
        }

        public void Setup(Notification notification)
        {
            _notification = notification;

            var style = _notification.Type switch
            {
                NotificationType.Info => Style.Default,
                NotificationType.Good => Style.Good,
                NotificationType.Bad => Style.Bad,
                _ => Style.Default
            };
            titleText.text = _notification.Title.WithStyle(style);
            descriptionText.text = _notification.Description;
            icon.gameObject.SetActive(_notification.Icon != null);
            icon.sprite = _notification.Icon;
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }

        private void PingNotification()
        {
            Pinged?.Invoke(_notification);
            Close();
        }
    }
}