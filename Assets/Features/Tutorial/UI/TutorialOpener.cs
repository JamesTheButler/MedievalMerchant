using Common.Infrastructure;
using Common.UI.Utility;
using Features.Tutorial.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Tutorial.UI
{
    public sealed class TutorialButton : MonoBehaviour
    {
        [SerializeField]
        private TutorialTopic tutorialTopic;

        [SerializeField, Required]
        private Button button;

        [SerializeField, Required]
        private Sprite defaultBackground, highlightedBackground;

        private TutorialService _tutorialService;

        private TMP_Text _buttonText;
        private Image _buttonImage;

        private void Start()
        {
            button.onClick.AddListener(Open);
            _tutorialService = GameplayContext.Instance.Services.TutorialService;
            _tutorialService.TopicCompletionChanged += OnTopicCompleted;
        }

        private void OnDestroy()
        {
            _tutorialService.TopicCompletionChanged -= OnTopicCompleted;
        }

        private void Open()
        {
            var service = GameplayContext.Instance.Services.TutorialService;
            service.OpenTutorial(tutorialTopic);
        }

        private void OnTopicCompleted(TutorialTopic topic, bool isCompleted)
        {
            if (topic != tutorialTopic)
                return;

            _buttonText.text = isCompleted
                ? "?".WithStyle(Style.TutorialHighlight)
                : "?".WithStyle(Style.Default);
        }
    }
}