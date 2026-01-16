using System;

namespace Common.Infrastructure.Observation
{
    public interface IReadOnlyObservableEvent
    {
        IBinding Observe(Action notifyCallback);
        void StopObserving(Action notifyCallback);
    }
}