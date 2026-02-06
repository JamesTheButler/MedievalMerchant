using Common.Infrastructure.Global;
using Common.UI.Elements;
using Common.UI.Elements.Panels;
using Common.UI.Utility;
using Features.Feedback.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Feedback.UI
{
    public sealed class FeedbackForm : DynamicPanel
    {
        private FeedbackService _feedbackService;

        [SerializeField, Required]
        private TMP_InputField nameInput, messageInput;

        [SerializeField, Required]
        private Button submitButton, cancelButton;

        protected override void OnInitialize()
        {
            submitButton.onClick.AddListener(Submit);
            cancelButton.onClick.AddListener(Close);

            _feedbackService = GlobalContext.Instance.Services.FeedbackService;
        }

        public void Submit()
        {
            if (messageInput.text == "")
                return;

            StartCoroutine(_feedbackService.PostFeedback(nameInput.text, messageInput.text));
            Close();
        }

        protected override void OnOpen()
        {
            nameInput.Clear();
            messageInput.Clear();

            gameObject.SetActive(true);

            nameInput.Select();
            nameInput.Select(); // ???
            nameInput.Select();
        }

        protected override void OnClose()
        {
            nameInput.Clear();
            messageInput.Clear();

            gameObject.SetActive(false);
        }
    }
}