using System;
using System.Collections.Generic;
using Common.Infrastructure.Observation;

namespace Common.Infrastructure.Modifiable
{
    /// <summary>
    /// Tracks all added observables and counts how many of them fulfill the given predicate.
    /// </summary>
    public sealed class ObservableFilter<T> : Observable<int>
    {
        private readonly Predicate<T> _predicate;
        private readonly List<Observable<T>> _observables = new();

        public ObservableFilter(Predicate<T> predicate) : this(Array.Empty<Observable<T>>(), predicate) { }

        public ObservableFilter(IEnumerable<Observable<T>> observables, Predicate<T> predicate)
        {
            _predicate = predicate;

            foreach (var observable in observables)
            {
                AddValue(observable);
            }
        }

        public void AddValue(Observable<T> value)
        {
            _observables.Add(value);
            value.Observe(EvaluatePredicate);

            if (_predicate.Invoke(value.Value))
            {
                Value++;
            }
        }

        public void RemoveValue(Observable<T> value)
        {
            if (!_observables.Contains(value))
                return;

            value.StopObserving(EvaluatePredicate);
            if (_predicate.Invoke(value.Value))
            {
                Value--;
            }
        }

        private void EvaluatePredicate(T oldValue, T newValue)
        {
            var doesOldValueFulfill = _predicate.Invoke(oldValue);
            var doesNewValueFulfill = _predicate.Invoke(newValue);

            if (doesOldValueFulfill == doesNewValueFulfill)
                return;

            if (doesNewValueFulfill)
            {
                Value++;
            }
            else
            {
                Value--;
            }
        }
    }
}