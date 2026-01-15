using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.UI;
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
        private UIBridgeService _uiBridgeService;

        private TutorialTopic? _currentTopic;

        private void Start()
        {
            _tutorialSResources = ResourceManager.Instance.TutorialResources;
            _tutorialService = GameplayContext.Instance.Services.TutorialService;
            _uiBridgeService = GameplayContext.Instance.Services.UIBridgeService;
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

            _currentTopic = topic;
            tutorialUI.Setup(topicData);
            tutorialUI.Open();
        }

        private void OnTutorialUiClosed()
        {
            if (_currentTopic == null)
                return;

            _uiBridgeService.CloseTutorialFromUI(_currentTopic.Value);
            _currentTopic = null;
        }
    }
}