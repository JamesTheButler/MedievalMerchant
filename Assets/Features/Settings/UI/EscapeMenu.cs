using Common.Infrastructure.Global;
using Common.UI.Elements.Panels;
using Features.Feedback.Logic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.Settings.UI
{
    public sealed class EscapeMenu : DynamicPanel
    {
        [SerializeField, Scene]
        private string startScene;

        [SerializeField]
        private UnityEvent giveUpPressed;

        [SerializeField, Required]
        private Button giveUpButton, cancelButton;

        private FeedbackService _feedbackService;

        protected override void OnInitialize()
        {
            _feedbackService = GlobalContext.Instance.Services.FeedbackService;

            cancelButton.onClick.AddListener(Close);
            giveUpButton.onClick.AddListener(GiveUp);
        }

        private static void OnFeedbackPosted()
        {
            Debug.Log("Feedback has been sent.");
        }

        protected override void OnOpen()
        {
            _feedbackService.FeedbackPosted.Observe(OnFeedbackPosted);

            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            _feedbackService.FeedbackPosted.StopObserving(OnFeedbackPosted);

            gameObject.SetActive(false);
        }

        private void GiveUp()
        {
            giveUpPressed.Invoke();
            SceneManager.LoadScene(startScene);
        }
    }
}