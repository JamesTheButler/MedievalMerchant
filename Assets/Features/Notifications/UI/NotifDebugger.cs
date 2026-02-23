using System.Collections.Generic;
using Common.Infrastructure.Gameplay;
using Features.Notifications.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Notifications.UI
{
    public sealed class NotifDebugger : MonoBehaviour
    {
        [SerializeField, Required]
        private Image backgroundImage;

        [SerializeField, Required]
        private TMP_Text text;

        private readonly List<Color> _colors = new()
        {
            Color.red,
            Color.green,
            Color.blue,
        };

        private int _runningIndex;

        private void Start()
        {
            GameplayContext.Instance.Services.NotificationService.NotificationPosted += OnNotifPosted;
        }

        private void OnNotifPosted(Notification notification)
        {
            text.text = notification.GetType().Name;
            backgroundImage.color = _colors[_runningIndex++ % _colors.Count];
        }
    }
}