using System;

namespace Common.Infrastructure.Observation
{
    public sealed class ObservableEvent<T1, T2> : IReadOnlyObservableEvent<T1, T2>
    {
        private event Action<T1, T2> Action;

        public void Invoke(T1 value1, T2 value2)
        {
            Action?.Invoke(value1, value2);
        }

        public IBinding Observe(Action<T1, T2> notifyCallback)
        {
            Action += notifyCallback;
            return new Binding(() => StopObserving(notifyCallback));
        }

        public void StopObserving(Action<T1, T2> notifyCallback)
        {
            Action -= notifyCallback;
        }
    }
}