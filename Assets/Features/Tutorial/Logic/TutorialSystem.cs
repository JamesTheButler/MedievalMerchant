using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Global;
using Common.Types;
using Common.UI;
using Common.UI.Elements;

namespace Features.Tutorial.Logic
{
    public sealed class TutorialSystem : ISystem
    {
        private TutorialService _tutorialService;
        private UIBridgeService _uiBridgeService;
        private GameplayModel _gameModel;

        public void Initialize()
        {
            _tutorialService = GameplayContext.Instance.Services.TutorialService;
            _uiBridgeService = GameplayContext.Instance.Services.UIBridgeService;
            _gameModel = GameplayContext.Instance.Model;

            _gameModel.DateModel.GameDate.Observe(OnDateChanged);
            _uiBridgeService.TutorialClosedFromUI += OnTutorialClosedFromUI;
            _uiBridgeService.PanelOpenedFromUI += OnPanelOpenedFromUI;
        }

        private void OnDateChanged(Date date)
        {
            var levelIndex = GlobalContext.CurrentLevelInfo?.InternalIndex ?? -1;
            if (date.Day != 2) // on day 2 we trigger all start-of-level tutorials
                return;

            switch (levelIndex)
            {
                case 0: // i.e. Level 1
                    OpenTutorialIfIncomplete(TutorialTopic.Intro);
                    break;
                case 1: // i.e. Level 2
                    OpenTutorialIfIncomplete(TutorialTopic.Retinue);
                    break;
            }
        }

        public void CleanUp()
        {
            _gameModel.DateModel.GameDate.StopObserving(OnDateChanged);
            _uiBridgeService.TutorialClosedFromUI -= OnTutorialClosedFromUI;
            _uiBridgeService.PanelOpenedFromUI -= OnPanelOpenedFromUI;
        }

        private void OnPanelOpenedFromUI(UIPanel uiPanel)
        {
            TutorialTopic? topic = uiPanel switch
            {
                UIPanel.Retinue => TutorialTopic.Retinue,
                UIPanel.Town => TutorialTopic.Town,
                _ => null,
            };

            if (topic == null)
                return;

            OpenTutorialIfIncomplete(topic.Value);
        }

        private void OnTutorialClosedFromUI(TutorialTopic topic)
        {
            if (topic == TutorialTopic.Intro)
            {
                OpenTutorialIfIncomplete(TutorialTopic.Controls);
            }
        }

        private void OpenTutorialIfIncomplete(TutorialTopic topic)
        {
            if (_tutorialService.CompletedChapters[topic])
                return;

            _tutorialService.OpenTutorial(topic);
        }
    }
}