using Common.Infrastructure.Observation;

namespace Features.Tutorial.Onboarding.Logic
{
    public sealed class OnboardingTask
    {
        public Observable<bool> IsCompleted { get; } = new();
        public string Message { get; }

        public OnboardingTask(string message)
        {
            Message = message;
        }
    }
}