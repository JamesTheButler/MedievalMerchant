using System;

namespace Common.Infrastructure.Observation
{
    public sealed class ObservableEvent<T1, T2, T3> : IReadOnlyObservableEvent<T1, T2, T3>
    {
        private event Action<T1, T2, T3> Action;

        public void Invoke(T1 value1, T2 value2, T3 value3)
        {
            Action?.Invoke(value1, value2, value3);
        }

        public IBinding Observe(Action<T1, T2, T3> notifyCallback)
        {
            Action += notifyCallback;
            return new Binding(() => StopObserving(notifyCallback));
        }

        public void StopObserving(Action<T1, T2, T3> notifyCallback)
        {
            Action -= notifyCallback;
        }
    }
}