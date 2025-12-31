using System;
using Common.UI;
using Features.Tutorial;

namespace Common.Infrastructure
{
    /// <summary>
    /// Forwards UI events, like user inputs to the logic layer.
    /// </summary>
    public sealed class UIBridgeService : IService
    {
        public event Action<UIPanel> PanelOpenedFromUI, PanelOpenedFromBackEnd;

        public event Action<TutorialTopic> TutorialClosedFromUI;

        public void Initialize() { }
        public void CleanUp() { }

        public void OpenPanelFromUI(UIPanel uiPanel)
        {
            PanelOpenedFromUI?.Invoke(uiPanel);
        }

        public void OpenPanelFromBackend(UIPanel uiPanel)
        {
            PanelOpenedFromBackEnd?.Invoke(uiPanel);
        }

        public void CloseTutorialFromUI(TutorialTopic topic)
        {
            TutorialClosedFromUI?.Invoke(topic);
        }
    }
}