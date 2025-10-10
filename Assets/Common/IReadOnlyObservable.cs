using System;

namespace Common
{
    public interface IReadOnlyObservable<out T>
    {
        T Value { get; }

        void Observe(Action<T> callback, bool invokeOnObserve = true);
        void StopObserving(Action<T> callback);

        void Observe(Action<T, T> callback);
        void StopObserving(Action<T, T> callback);
    }
}