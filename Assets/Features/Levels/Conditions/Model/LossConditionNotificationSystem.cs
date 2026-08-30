using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Features.Notifications.Logic;

namespace Features.Levels.Conditions.Model
{
    public sealed class LossConditionNotificationSystem : ISystem
    {
        private NotificationService _notificationService;
        private LevelConditions _levelConditions;

        private readonly Bindings _bindings = new();

        public void Initialize()
        {
            _levelConditions = GameplayContext.Instance.Model.Conditions;
            _notificationService = GameplayContext.Instance.Services.NotificationService;

            foreach (var lossCondition in _levelConditions.LossConditions)
            {
                _bindings.Track(
                    lossCondition.IsClose.Observe(isClose => OnIsCloseChanged(isClose, lossCondition))
                );
            }
        }

        private void OnIsCloseChanged(bool isClose, ILossCondition lossCondition)
        {
            if (!isClose) return;

            var notification = new LossConditionNotification(lossCondition);
            _notificationService.PostNotification(notification);
        }

        public void CleanUp()
        {
            _bindings.Unbind();
        }
    }
}