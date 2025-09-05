using System;

namespace Common
{
    public interface IReadOnlyObservable<out T>
    {
        T Value { get; }
        
        void Observe(Action<T> callback, bool invokeOnObserve = true);
        public void StopObserving(Action<T> callback);
    }
}