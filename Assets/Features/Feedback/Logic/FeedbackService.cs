using Common.Infrastructure;
using Common.Infrastructure.Global;
using Common.Infrastructure.Observation;
using Sentry.Unity;

namespace Features.Feedback.Logic
{
    public sealed class FeedbackService : IService
    {
        public ObservableEvent FeedbackPosted = new();

        public void PostFeedback(string senderName, string feedback)
        {
            var levelName = GlobalContext.CurrentLevelInfo?.DisplayIndex.ToString() ?? "StartScreen";

            SentrySdk.ConfigureScope(scope => { scope.SetTag("level_id", levelName); });
            SentrySdk.CaptureFeedback(name: senderName, message: feedback);

            FeedbackPosted?.Invoke();
        }

        public void Initialize() { }
        public void CleanUp() { }
    }
}