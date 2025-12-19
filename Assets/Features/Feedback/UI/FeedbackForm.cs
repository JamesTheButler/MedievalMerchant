using System;
using Common.UI.Utility;
using Features.Feedback.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Feedback.UI
{
    public sealed class FeedbackForm : MonoBehaviour
    {
        private readonly FeedbackService _feedbackService = new();

        [SerializeField, Required]
        private TMP_InputField nameInput, messageInput;

        [SerializeField, Required]
        private Button submitButton, cancelButton;

        private void Awake()
        {
            submitButton.onClick.AddListener(Open);
            cancelButton.onClick.AddListener(Close);
        }

        public void Submit()
        {
            StartCoroutine(_feedbackService.PostFeedback(nameInput.text, messageInput.text));
            Close();
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

        public void Toggle()
        {
            if (gameObject.activeSelf)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }
}