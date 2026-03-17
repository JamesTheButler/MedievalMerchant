using Common.Infrastructure;
using Features.Notifications.Logic;
using UnityEngine;

namespace Features.Levels.Conditions.Model
{
    public sealed record LossConditionNotification(ILossCondition LossCondition) : Notification(
        GetTitle(),
        LossCondition.WarningMessage,
        NotificationType.Bad,
        Severity.Major,
        GetIcon(LossCondition))
    {
        private static string GetTitle()
        {
            var loc = ResourceManager.Instance.LocalizationResources.NotificationResources;
            return loc.LossImminentNotificationTitle.GetLocalizedString();
        }

        private static Sprite GetIcon(ILossCondition lossCondition)
        {
            var conditionResources = ResourceManager.Instance.ConditionResources;
            return conditionResources.Conditions[lossCondition.Type].Icon;
        }
    }
}