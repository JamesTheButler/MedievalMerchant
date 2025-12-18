using System;
using Common.Infrastructure;
using Common.Types;
using TMPro;
using UnityEngine;

namespace Common.UI
{
    public sealed class DateGauge : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text dateText;

        private readonly Lazy<Date> _date = new(() => GameplayContext.Instance.Model.Date);

        private const string DateFormat = "Year {0}. Day {1}";
        private int _day, _year;

        private void Start()
        {
            _date.Value.Day.Observe(OnDayChanged);
            _date.Value.Year.Observe(OnYearChanged);
        }

        private void OnDestroy()
        {
            _date.Value.Day.StopObserving(OnDayChanged);
            _date.Value.Year.StopObserving(OnYearChanged);
        }

        private void OnYearChanged(int year)
        {
            _year = year;
            UpdateText();
        }

        private void OnDayChanged(int day)
        {
            _day = day;
            UpdateText();
        }

        private void UpdateText()
        {
            dateText.text = string.Format(DateFormat, _year, _day);
        }
    }
}