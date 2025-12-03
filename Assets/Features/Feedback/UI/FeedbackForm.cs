using Common.UI;
using Features.Feedback.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Feedback.UI
{
    public sealed class FeedbackForm : MonoBehaviour
    {
        private readonly FeedbackService _feedbackService = new();

        [SerializeField, Required]
        private TMP_InputField nameInput, messageInput;

        public void Submit()
        {
            StartCoroutine(_feedbackService.PostFeedback(nameInput.text, messageInput.text));
            Close();
        }

        public void NavigateNext(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            messageInput.Select();
        }

        public void Open()
        {
            nameInput.Clear();
            messageInput.Clear();

            gameObject.SetActive(true);

            nameInput.Select();
            nameInput.Select();
            nameInput.Select(); 
        }

        public void Close()
        {
            nameInput.Clear();
            messageInput.Clear();

            gameObject.SetActive(false);
        }
    }
}