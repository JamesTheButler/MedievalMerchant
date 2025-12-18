using Common.Infrastructure;
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

        private void Start()
        {
            _tutorialSResources = ResourceManager.Instance.TutorialResources;
            _tutorialService = GameplayContext.Instance.Services.TutorialService;
            _tutorialService.OpenTutorialRequest += OpenTutorial;
        }

        private void OnDestroy()
        {
            _tutorialService.OpenTutorialRequest -= OpenTutorial;
        }

        private void OpenTutorial(TutorialTopic topic)
        {
            var topicData = _tutorialSResources.Topics[topic];
            if (topicData == null)
            {
                Debug.LogError($"Could not find topic data for '{topic}'");
            }

            tutorialUI.Setup(topicData);
        }
    }
}