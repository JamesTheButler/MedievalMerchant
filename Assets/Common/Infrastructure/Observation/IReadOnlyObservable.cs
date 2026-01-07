using System;

namespace Common.Infrastructure.Observation
{
    public interface IReadOnlyObservable<out T>
    {
        T Value { get; }
        
        IBinding Observe(Action<T> callback, bool invokeOnObserve = true);
        void StopObserving(Action<T> callback);

        IBinding Observe(Action<T, T> callback);
        void StopObserving(Action<T, T> callback);
        
        IBinding Observe(Action callback);
        void StopObserving(Action callback);
    }
}