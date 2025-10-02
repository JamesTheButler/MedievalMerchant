using System;

namespace Common
{
    public class Observable<T> : IReadOnlyObservable<T>
    {
        private event Action<T> ValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (_value?.Equals(value) ?? false) return;

                _value = value;
                ValueChanged?.Invoke(value);
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

        public void StopObserving(Action<T> callback)
        {
            ValueChanged -= callback;
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