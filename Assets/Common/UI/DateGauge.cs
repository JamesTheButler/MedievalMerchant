using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements;
using Features.Localization.UI;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UI
{
    public sealed class DateGauge : InitializableBehavior
    {
        [SerializeField, Required]
        private LocalizedText dateText;

        private DateModel _gameDate;

        public override void Initialize()
        {
            _gameDate = GameplayContext.Instance.Model.DateModel;
            _gameDate.GameDate.Observe(OnDateChanged);
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _gameDate.GameDate.StopObserving(OnDateChanged);
        }

        private void OnDateChanged(Date date)
        {
            var args = new
            {
                _int_Day = date.Day,
                _int_Year = date.Year,
            };
            dateText.SetArgs(args);
        }
    }
}