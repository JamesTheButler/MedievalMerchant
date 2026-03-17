using System;
using Common.UI.Elements.Panels;
using Common.UI.Utility;
using Features.Notifications.Logic;
using Features.Tutorial.Onboarding.UI;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Notifications.UI
{
    public sealed class MajorNotificationPanel : DynamicPanel
    {
        public event Action<Notification> Pinged;

        [SerializeField, Required]
        private TMP_Text titleText, descriptionText;

        [SerializeField, Required]
        private Image icon;

        [SerializeField, Required]
        private Button closeButton, pingButton;

        [SerializeField, Required]
        private PopupOpenCloseAnimatorHandler animatorHandler;

        private Notification _notification;

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

        protected override void OnInitialize()
        {
            closeButton.onClick.AddListener(Close);
            pingButton.onClick.AddListener(PingNotification);
            animatorHandler.OnClosed += OnClosedAnimationCompleted;
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
            animatorHandler.StartOpenAnimation();
        }

        protected override void OnClose()
        {
            animatorHandler.StartCloseAnimation();
        }

        private void OnClosedAnimationCompleted()
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