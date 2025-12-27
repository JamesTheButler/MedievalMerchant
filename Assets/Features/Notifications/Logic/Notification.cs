using JetBrains.Annotations;
using UnityEngine;

namespace Features.Notifications.Logic
{
    public abstract record Notification(
        string Title,
        string Description,
        [CanBeNull]
        Sprite Icon);
}