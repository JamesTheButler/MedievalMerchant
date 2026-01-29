using Common.Infrastructure.Observation;

namespace Features.Tutorial.UI
{
    public sealed class TutorialTask
    {
        public Observable<bool> IsCompleted { get; } = new();
        public string Message { get; }

        public TutorialTask(string message)
        {
            Message = message;
        }
    }
}