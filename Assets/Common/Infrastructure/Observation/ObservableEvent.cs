using System;

namespace Common.Infrastructure.Observation
{
    public sealed class ObservableEvent : IReadOnlyObservableEvent
    {
        private event Action Action;

        public void Invoke()
        {
            Action?.Invoke();
        }

        public IBinding Observe(Action notifyCallback)
        {
            Action += notifyCallback;
            return new Binding(() => StopObserving(notifyCallback));
        }

        public void StopObserving(Action notifyCallback)
        {
            Action -= notifyCallback;
        }
    }
}