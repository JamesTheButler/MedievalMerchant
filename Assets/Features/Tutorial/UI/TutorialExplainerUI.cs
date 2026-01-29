using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Tutorial.UI
{
    public sealed class TutorialExplainerUI : MonoBehaviour
    {
        [SerializeField, Required]
        public TMP_Text explainerText;

        [SerializeField, Required]
        private Button nextButton;

        public event Action NextClicked;

        public void SetUp(string message)
        {
            explainerText.text = message;
            nextButton.onClick.AddListener(() => NextClicked?.Invoke());
        }
    }
}