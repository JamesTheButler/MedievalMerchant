using System;
using System.Collections.Generic;

namespace Common.Infrastructure.Observation
{
    public abstract class ReadOnlyObservable<T> : IReadOnlyObservable<T>
    {
        private static readonly EqualityComparer<T> Comparer = EqualityComparer<T>.Default;

        private event Action ValueChangeWithoutValue;
        private event Action<T> ValueChanged;
        private event Action<T, T> ValueChangedWithOldValue;

        public T Value { get; private set; }

        protected ReadOnlyObservable(T value = default)
        {
            SetValue(value);
        }

        protected void SetValue(T value)
        {
            if (Comparer.Equals(Value, value)) return;

            var oldValue = Value;
            Value = value;
            ValueChanged?.Invoke(Value);
            ValueChangedWithOldValue?.Invoke(oldValue, Value);
            ValueChangeWithoutValue?.Invoke();
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

        public static implicit operator T(ReadOnlyObservable<T> observable)
        {
            return observable.Value;
        }

        public override string ToString()
        {
            return $">{Value}<";
        }
    }
}
