using System;
using Data;
using TMPro;
using UnityEngine;

namespace UI
{
    public sealed class DateGauge : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text dateText;

        private readonly Lazy<Date> _date = new(() => Model.Instance.Date);

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