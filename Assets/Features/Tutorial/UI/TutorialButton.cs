using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Global;
using Common.UI.Tooltips;
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
        private SimpleTooltipHandler tooltip;

        [SerializeField, Required]
        private Sprite defaultBackground, highlightedBackground;

        private TutorialService _tutorialService;

        private TMP_Text _buttonText;
        private Image _buttonImage;

        private void Start()
        {
            button.onClick.AddListener(Open);
            _buttonImage = button.GetComponentInChildren<Image>();
            _buttonText = button.GetComponentInChildren<TMP_Text>();
            _tutorialService = GlobalContext.Instance.Services.TutorialService;
            _tutorialService.TopicCompletionChanged += OnTopicCompleted;
            var topicName = ResourceManager.Instance.TutorialResources.Topics[tutorialTopic].Title;
            tooltip.SetData($"Tutorial: {topicName}");
            MarkAsCompleted(_tutorialService.CompletedChapters[tutorialTopic]);
        }

        private void OnDestroy()
        {
            if (_tutorialService == null)
                return;
            _tutorialService.TopicCompletionChanged -= OnTopicCompleted;
        }

        private void Open()
        {
            var service = GlobalContext.Instance.Services.TutorialService;
            service.OpenTutorial(tutorialTopic);
        }

        private void OnTopicCompleted(TutorialTopic topic, bool isCompleted)
        {
            if (topic != tutorialTopic)
                return;

            MarkAsCompleted(isCompleted);
        }

        private void MarkAsCompleted(bool isCompleted)
        {
            _buttonText.text = isCompleted
                ? "?".WithStyle(Style.Default)
                : "?".WithStyle(Style.TutorialHighlight);

            _buttonImage.sprite = isCompleted
                ? defaultBackground
                : highlightedBackground;
        }
    }
}