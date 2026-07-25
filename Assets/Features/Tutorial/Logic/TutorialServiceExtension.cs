namespace Features.Tutorial.Logic
{
    public static class TutorialServiceExtension
    {
        public static bool IsCompleted(this TutorialService service, TutorialTopic topic)
        {
            return service.CompletedChapters[topic];
        }

        public static bool TryOpenFirstTime(this TutorialService service, TutorialTopic topic)
        {
            var isFirstTime = !service.IsCompleted(topic);

            if (isFirstTime)
            {
                service.OpenTutorial(topic);
            }

            return isFirstTime;
        }
    }
}