using System;
using Features.Tutorial;

namespace Common.Infrastructure
{
    /// <summary>
    /// Forwards UI events, like user inputs to the logic layer.
    /// </summary>
    public sealed class UIEventService : IService
    {
        public event Action RetinuePanelOpened, CaravanPanelOpened, TownPanelOpened;
        public event Action<TutorialTopic> TutorialClosed;

        public void Initialize() { }
        public void CleanUp() { }

        public void OpenRetinuePanel()
        {
            RetinuePanelOpened?.Invoke();
        }

        public void OpenCaravanPanel()
        {
            CaravanPanelOpened?.Invoke();
        }

        public void OpenTownPanel()
        {
            TownPanelOpened?.Invoke();
        }

        public void CloseTutorial(TutorialTopic topic)
        {
            TutorialClosed?.Invoke(topic);
        }
    }
}