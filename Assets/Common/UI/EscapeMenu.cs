using Common.Infrastructure;
using Common.UI.Elements;
using Features.Tutorial.Logic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Common.UI
{
    public sealed class EscapeMenu : DynamicPanel
    {
        [SerializeField, Scene]
        private string startScene;

        [SerializeField]
        private UnityEvent giveUpPressed, feedbackButtonPressed;

        [SerializeField, Required]
        private Button giveUpButton, resetTutorialButton, feedbackButton, cancelButton;

        private TutorialService _tutorialService;

        protected override void OnInitialize()
        {
            _tutorialService = GameplayContext.Instance.Services.TutorialService;

            cancelButton.onClick.AddListener(Close);
            giveUpButton.onClick.AddListener(GiveUp);
            resetTutorialButton.onClick.AddListener(ResetTutorialState);
            feedbackButton.onClick.AddListener(ReportBug);
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }

        private void ReportBug()
        {
            feedbackButtonPressed.Invoke();
        }

        private void ResetTutorialState()
        {
            _tutorialService.ResetCompletedTopics();
        }

        private void GiveUp()
        {
            giveUpPressed.Invoke();
            SceneManager.LoadScene(startScene);
        }
    }
}