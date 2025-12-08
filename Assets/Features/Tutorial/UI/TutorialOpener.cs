using Infrastructure;
using UnityEngine;

namespace Features.Tutorial.UI
{
    public sealed class TutorialOpener : MonoBehaviour
    {
        [SerializeField] private TutorialTopic tutorialTopic;

        public void Open()
        {
            var service = GameplayContext.Instance.Services.TutorialService;
            service.OpenTutorial(tutorialTopic);
        }
    }
}