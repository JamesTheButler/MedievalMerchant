using System;
using System.Collections.Generic;

namespace Common.Modifiable
{
    public sealed class ObservableAverage : Observable<float>
    {
        private readonly HashSet<Observable<float>> _values = new();

        public void AddValue(Observable<float> value)
        {
            if (!_values.Add(value))
                throw new ArgumentException($"Value already added. {value}");

            Value += value.Value;
            value.Observe(Refresh);
        }

        public void RemoveValue(Observable<float> value)
        {
            if (!_values.Contains(value))
                return;
            _values.Remove(value);
            value.StopObserving(Refresh);
            Value -= value.Value;
        }

        private void Refresh(float oldValue, float newValue)
        {
            Value = Value - oldValue + newValue;
        }
    }
}