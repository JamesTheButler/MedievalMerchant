using System;
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
        public event Action Opened, Closed;

        [SerializeField, Scene]
        private string startScene;

        [SerializeField]
        private UnityEvent giveUpPressed, feedbackButtonPressed;

        [SerializeField, Required]
        private Button giveUpButton, resetTutorialButton, feedbackButton, cancelButton;

        private void Start()
        {
            cancelButton.onClick.AddListener(Hide);
            giveUpButton.onClick.AddListener(GiveUp);
            resetTutorialButton.onClick.AddListener(ResetTutorialState);
            feedbackButton.onClick.AddListener(ReportBug);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            Opened?.Invoke();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            Closed?.Invoke();
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

        public void Toggle()
        {
            if (gameObject.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
}