using Common.Infrastructure;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Common.UI
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
        private Button giveUpButton, resetTutorialButton, feedbackButton, cancelButton;

        private void Start()
        {
            cancelButton.onClick.AddListener(CloseSelf);
            giveUpButton.onClick.AddListener(GiveUp);
            resetTutorialButton.onClick.AddListener(ResetTutorialState);
            feedbackButton.onClick.AddListener(ReportBug);
        }

        private void CloseSelf()
        {
            gameObject.SetActive(false);
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