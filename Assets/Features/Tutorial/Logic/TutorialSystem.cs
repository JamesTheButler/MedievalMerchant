using Common.Infrastructure;

namespace Features.Tutorial.Logic
{
    public sealed class TutorialSystem : ISystem
    {
        private TutorialService _tutorialService;
        private UIEventService _uiEventService;
        private GameplayModel _gameModel;

        public void Initialize()
        {
            _tutorialService = GameplayContext.Instance.Services.TutorialService;
            _uiEventService = GameplayContext.Instance.Services.UIEventService;
            _gameModel = GameplayContext.Instance.Model;

            _gameModel.Date.Day.Observe(OnDayChanged);
            _uiEventService.TutorialClosed += OnTutorialClosed;
            _uiEventService.CaravanPanelOpened += OnCaravanPanelOpened;
            _uiEventService.TownPanelOpened += OnTownPanelOpened;
            _uiEventService.RetinuePanelOpened += OnRetinuePanelOpened;
        }

        private void OnDayChanged(int day)
        {
            var levelIndex = GlobalContext.CurrentLevelInfo?.InternalIndex ?? -1;
            if (day != 2) // on day 2 we trigger all start-of-level tutorials
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
            _gameModel.Date.Day.StopObserving(OnDayChanged);
            _uiEventService.TutorialClosed -= OnTutorialClosed;
            _uiEventService.CaravanPanelOpened -= OnCaravanPanelOpened;
            _uiEventService.TownPanelOpened -= OnTownPanelOpened;
            _uiEventService.RetinuePanelOpened -= OnRetinuePanelOpened;
        }

        private void OnCaravanPanelOpened()
        {
            OpenTutorialIfIncomplete(TutorialTopic.Caravan);
        }

        private void OnTownPanelOpened()
        {
            OpenTutorialIfIncomplete(TutorialTopic.Town);
        }

        private void OnRetinuePanelOpened()
        {
            OpenTutorialIfIncomplete(TutorialTopic.Retinue);
        }

        private void OnTutorialClosed(TutorialTopic topic)
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