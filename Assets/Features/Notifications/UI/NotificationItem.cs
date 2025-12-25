    using System;
using Features.Notifications.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Notifications.UI
{
    public sealed class NotificationItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private TMP_Text titleText, descriptionText;

        [SerializeField]
        private Button xButton;

        private Action _pingCallback, _destroyCallback;

        public void SetUp(Notification notification, Action pingCallback, Action destroyCallback)
        {
            _pingCallback = pingCallback;
            _destroyCallback = destroyCallback;
            titleText.text = notification.Title;
            descriptionText.text = notification.Description;
            xButton.onClick.AddListener(Close);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _pingCallback.Invoke();
        }

        public void Close()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _destroyCallback.Invoke();
        }
    }
}