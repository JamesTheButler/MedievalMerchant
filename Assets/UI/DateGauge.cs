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
            _date.Value.DayChanged += OnDayChanged;
            _date.Value.YearChanged += OnYearChanged;
            
            OnDayChanged(_date.Value.Day);
            OnYearChanged(_date.Value.Year);
        }

        private void OnDestroy()
        {
            _date.Value.DayChanged -= OnDayChanged;
            _date.Value.YearChanged -= OnYearChanged;
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