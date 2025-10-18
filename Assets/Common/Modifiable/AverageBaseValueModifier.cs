using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Modifiable
{
    public sealed class AverageBaseValueModifier : BaseValueModifier
    {
        private readonly HashSet<Observable<float>> _values = new();

        public AverageBaseValueModifier(string propertyName) : base(0, $"Average {propertyName}") { }

        public void AddValue(Observable<float> value)
        {
            if (!_values.Add(value))
                throw new ArgumentException($"Value already added. {value}");

            value.Observe(OnValueChanged);
        }

        public void RemoveValue(Observable<float> value)
        {
            if (!_values.Contains(value))
                return;

            _values.Remove(value);
            value.StopObserving(OnValueChanged);
            Refresh();
        }

        private void OnValueChanged(float _)
        {
            Refresh();
        }

        private void Refresh()
        {
            Value.Value = _values.Average(observable => observable.Value);
        }
    }
}