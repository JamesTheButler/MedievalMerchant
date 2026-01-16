using System;

namespace Common.Infrastructure.Observation
{
    public sealed class ObservableEvent<T> : IReadOnlyObservableEvent<T>
    {
        private event Action<T> Action;

        public void Invoke(T value)
        {
            Action?.Invoke(value);
        }

        public IBinding Observe(Action<T> notifyCallback)
        {
            Action += notifyCallback;
            return new Binding(() => StopObserving(notifyCallback));
        }

        public void StopObserving(Action<T> notifyCallback)
        {
            Action -= notifyCallback;
        }
    }
}