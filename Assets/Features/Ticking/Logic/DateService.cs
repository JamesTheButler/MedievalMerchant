using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;

namespace Features.Ticking.Logic
{
    public sealed class DateSystem : ISystem
    {
        private TickingService _tickingService;
        private DateModel _dateModel;

        public void Initialize()
        {
            _tickingService = GameplayContext.Instance.Services.TickingService;
            _tickingService.DayPassed += OnDayChanged;

            _dateModel = GameplayContext.Instance.Model.DateModel;
        }

        public void CleanUp()
        {
            _tickingService.DayPassed -= OnDayChanged;
        }

        private void OnDayChanged()
        {
            _dateModel.Increment();
        }
    }
}