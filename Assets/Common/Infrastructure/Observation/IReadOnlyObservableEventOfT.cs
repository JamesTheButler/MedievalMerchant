using System;

namespace Common.Infrastructure.Observation
{
    public interface IReadOnlyObservableEvent<out T>
    {
        IBinding Observe(Action<T> notifyCallback);
        void StopObserving(Action<T> notifyCallback);
    }
}