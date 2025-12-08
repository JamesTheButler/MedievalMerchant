using System;

namespace Features.Tutorial.Logic
{
    public sealed class TutorialService
    {
        public event Action<TutorialTopic> OpenTutorialRequest;
        
        public void OpenTutorial(TutorialTopic topic)
        {
            OpenTutorialRequest?.Invoke(topic);
        }
    }
}