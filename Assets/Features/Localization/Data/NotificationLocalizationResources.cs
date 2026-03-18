using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class NotificationLocalizationResources
    {
        [field: SerializeField]
        public LocalizedString LossImminentNotificationTitle { get; private set; }

        [field: SerializeField]
        public LocalizedString EventExpiredNotification { get; private set; }

        [field: SerializeField]
        public LocalizedString EventStartedNotification { get; private set; }
    }
}