using System;

namespace Common.Infrastructure.Observation
{
    public interface IReadOnlyObservableEvent
    {
        IBinding Observe(Action notifyCallback);
        void StopObserving(Action notifyCallback);
    }
    
    public interface IReadOnlyObservableEvent<out T>
    {
        IBinding Observe(Action<T> notifyCallback);
        void StopObserving(Action<T> notifyCallback);
    }
    
    public interface IReadOnlyObservableEvent<out T1, out T2>
    {
        IBinding Observe(Action<T1, T2> notifyCallback);
        void StopObserving(Action<T1, T2> notifyCallback);
    }
    
    public interface IReadOnlyObservableEvent<out T1, out T2, out T3>
    {
        IBinding Observe(Action<T1, T2, T3> notifyCallback);
        void StopObserving(Action<T1, T2, T3> notifyCallback);
    }
}