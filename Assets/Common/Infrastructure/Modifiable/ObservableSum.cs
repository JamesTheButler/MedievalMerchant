using System.Collections.Generic;
using Common.Infrastructure.Observation;

namespace Common.Infrastructure.Modifiable
{
    public sealed class ObservableSum : Observable<float>
    {
        private readonly List<Observable<float>> _observables = new();

        public void AddValue(Observable<float> value)
        {
            _observables.Add(value);
            value.Observe(Refresh);
            Value += value.Value;
        }

        public void RemoveValue(Observable<float> value)
        {
            if (!_observables.Contains(value))
                return;

            value.StopObserving(Refresh);
            Value -= value.Value;
        }

        private void Refresh(float oldValue, float newValue)
        {
            Value = Value - oldValue + newValue;
        }
    }
}