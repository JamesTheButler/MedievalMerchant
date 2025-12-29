using Common.Infrastructure;
using Features.Ticking;
using Features.Ticking.Logic;
using Features.Tutorial.Data;
using Features.Tutorial.Logic;
using UnityEngine;

namespace Features.Tutorial.UI
{
    public sealed class TutorialUIHandler : MonoBehaviour
    {
        [SerializeField]
        private TutorialUI tutorialUI;

        private TutorialResources _tutorialSResources;
        private TutorialService _tutorialService;
        private GameSpeedModel _gameSpeedModel;

        private void Start()
        {
            _tutorialSResources = ResourceManager.Instance.TutorialResources;
            _tutorialService = GameplayContext.Instance.Services.TutorialService;
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _tutorialService.OpenTutorialRequest += OpenTutorial;
            tutorialUI.Closed += OnTutorialUiClosed;
        }

        private void OnDestroy()
        {
            _tutorialService.OpenTutorialRequest -= OpenTutorial;
            tutorialUI.Closed -= OnTutorialUiClosed;
        }

        private void OpenTutorial(TutorialTopic topic)
        {
            var topicData = _tutorialSResources.Topics[topic];
            if (topicData == null)
            {
                Debug.LogError($"Could not find topic data for '{topic}'");
            }

            tutorialUI.Setup(topicData);
            tutorialUI.Open();
            _gameSpeedModel.Pause();
        }

        private void OnTutorialUiClosed()
        {
            _gameSpeedModel.Resume();
        }
    }
}