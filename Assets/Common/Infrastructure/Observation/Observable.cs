using System;
using System.Collections.Generic;

namespace Common.Infrastructure.Observation
{
    public class Observable<T> : IReadOnlyObservable<T>
    {
        private static readonly EqualityComparer<T> Comparer = EqualityComparer<T>.Default;

        private event Action ValueChangeWithoutValue;
        private event Action<T> ValueChanged;
        private event Action<T, T> ValueChangedWithOldValue;

        public T Value
        {
            get => _value;
            set
            {
                if (Comparer.Equals(_value, value)) return;

                var oldValue = _value;
                _value = value;
                ValueChanged?.Invoke(_value);
                ValueChangedWithOldValue?.Invoke(oldValue, _value);
                ValueChangeWithoutValue?.Invoke();
            }
        }

        private T _value;

        public Observable(T value = default)
        {
            Value = value;
        }

        public IBinding Observe(Action notifyCallback, bool invokeOnObserve)
        {
            ValueChangeWithoutValue += notifyCallback;
            if (invokeOnObserve)
            {
                notifyCallback?.Invoke();
            }

            return new Binding(() => StopObserving(notifyCallback));
        }

        public IBinding Observe(Action<T> callback, bool invokeOnObserve = true)
        {
            ValueChanged += callback;
            if (invokeOnObserve)
            {
                callback?.Invoke(Value);
            }

            return new Binding(() => StopObserving(callback));
        }

        public IBinding Observe(Action<T, T> callback)
        {
            ValueChangedWithOldValue += callback;
            return new Binding(() => StopObserving(callback));
        }

        public void StopObserving(Action notifyCallback)
        {
            ValueChangeWithoutValue -= notifyCallback;
        }

        public void StopObserving(Action<T> callback)
        {
            ValueChanged -= callback;
        }

        public void StopObserving(Action<T, T> callback)
        {
            ValueChangedWithOldValue -= callback;
        }

        public static implicit operator T(Observable<T> observable)
        {
            return observable.Value;
        }

        public override string ToString()
        {
            return $">{Value}<";
        }
    }
}