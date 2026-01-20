using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Common.UI
{
    public sealed class DateGauge : InitializableBehavior
    {
        [SerializeField, Required]
        private TMP_Text dateText;

        private DateModel _gameDate;

        private const string DateFormat = "Year {0}. Day {1}";

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
            dateText.text = string.Format(DateFormat, date.Year, date.Day);
        }
    }
}