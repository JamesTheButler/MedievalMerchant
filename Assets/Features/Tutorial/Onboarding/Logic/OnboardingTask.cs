using Common.Infrastructure.Observation;

namespace Features.Tutorial.Onboarding.Logic
{
    public sealed class OnboardingTask
    {
        public string Message { get; }

        public IReadOnlyObservable<bool> IsCompleted => _isCompleted;

        private readonly Observable<bool> _isCompleted = new();

        public OnboardingTask(string message)
        {
            Message = message;
        }

        public void Complete()
        {
            _isCompleted.Value = true;
        }
    }
}