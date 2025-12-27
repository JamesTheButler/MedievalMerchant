using System;
using System.Collections;
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
        private Image icon;

        [SerializeField]
        private Button xButton;

        private Action _pingCallback, _destroyCallback;
        private float _lifetimeInSec;
        private Coroutine _closeCoroutine;

        public void SetUp(Notification notification, float lifetimeInSec, Action pingCallback, Action destroyCallback)
        {
            _pingCallback = pingCallback;
            _destroyCallback = destroyCallback;
            _lifetimeInSec = lifetimeInSec;

            titleText.text = notification.Title;
            descriptionText.text = notification.Description;
            icon.gameObject.SetActive(notification.Icon != null);
            icon.sprite = notification.Icon;
            xButton.onClick.AddListener(Close);
            StartLifetimeTimer();
        }

        public void Close()
        {
            if (_closeCoroutine != null)
            {
                StopCoroutine(_closeCoroutine);
                _closeCoroutine = null;
            }

            _destroyCallback.Invoke();
            Destroy(gameObject);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _pingCallback.Invoke();
        }

        private void StartLifetimeTimer()
        {
            if (_closeCoroutine != null)
            {
                StopCoroutine(_closeCoroutine);
            }

            _closeCoroutine = StartCoroutine(CloseAfterSeconds(_lifetimeInSec));
        }

        private IEnumerator CloseAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Close();
        }
    }
}