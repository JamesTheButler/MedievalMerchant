using Features.Ticking;
using Infrastructure;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public sealed class EscapeMenu : MonoBehaviour
    {
        [SerializeField, Scene]
        private string startScene;

        [SerializeField]
        private UnityEvent giveUpPressed;

        [SerializeField]
        private UnityEvent feedbackButtonPressed;

        [SerializeField, Required]
        private Button giveUpButton, resetTutorialButton, feedbackButton;

        private void Start()
        {
            giveUpButton.onClick.AddListener(GiveUp);
            resetTutorialButton.onClick.AddListener(ResetTutorialState);
            feedbackButton.onClick.AddListener(ReportBug);
        }

        private void OnDestroy()
        {
            giveUpButton.onClick.RemoveListener(GiveUp);
            resetTutorialButton.onClick.RemoveListener(ResetTutorialState);
            feedbackButton.onClick.RemoveListener(ReportBug);
        }

        private void ReportBug()
        {
            feedbackButtonPressed.Invoke();
        }

        private void ResetTutorialState()
        {
            GameplayContext.Instance.Services.TutorialService.ResetCompletedTopics();
        }

        private void GiveUp()
        {
            giveUpPressed.Invoke();
            SceneManager.LoadScene(startScene);
        }
    }
}