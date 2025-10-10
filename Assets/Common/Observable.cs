using System;

namespace Common
{
    public class Observable<T> : IReadOnlyObservable<T>
    {
        private event Action<T> ValueChanged;
        private event Action<T, T> ValueChangedWithOldValue;

        public T Value
        {
            get => _value;
            set
            {
                if (_value?.Equals(value) ?? false) return;

                var oldValue = _value;
                _value = value;
                ValueChanged?.Invoke(_value);
                ValueChangedWithOldValue?.Invoke(oldValue, _value);
            }
        }

        private T _value;

        public Observable(T value = default)
        {
            Value = value;
        }

        public void Observe(Action<T> callback, bool invokeOnObserve = true)
        {
            ValueChanged += callback;
            if (invokeOnObserve)
            {
                callback?.Invoke(Value);
            }
        }

        public void Observe(Action<T, T> callback)
        {
            ValueChangedWithOldValue += callback;
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