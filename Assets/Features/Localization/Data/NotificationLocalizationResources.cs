using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class NotificationLocalizationResources
    {
        [field: SerializeField]
        public LocalizedString LossImminentNotification { get; private set; }
    }
}