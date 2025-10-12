using System;
using Common;
using UnityEngine;

namespace Features.Levels.Config.Conditions
{
    public sealed class Progress
    {
        public readonly Observable<bool> IsCompleted = new();
        public readonly Observable<int> CurrentValue = new();
        public readonly Observable<float> CurrentValuePercent = new();
        public readonly Observable<string> CurrentValueText = new();

        private readonly int _maxValue;

        private readonly Func<int, int, string> _formatter;

        public Progress(int maxValue, Func<int, int, string> formatter)
        {
            _formatter = formatter;
            _maxValue = maxValue;
            SetProgress(0);
        }

        public void SetProgress(int currentValue)
        {
            CurrentValue.Value = Mathf.Clamp(currentValue, 0, _maxValue);
            CurrentValuePercent.Value = (float)CurrentValue.Value / _maxValue;
            IsCompleted.Value = currentValue >= _maxValue;
            CurrentValueText.Value = _formatter.Invoke(CurrentValue.Value, _maxValue);
        }
    }
}