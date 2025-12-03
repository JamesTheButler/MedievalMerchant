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
        private Button giveUpButton;

        [SerializeField, Required]
        private Button feedbackButton;

        private void Start()
        {
            giveUpButton.onClick.AddListener(GiveUp);
            feedbackButton.onClick.AddListener(ReportBug);
        }

        private void OnDestroy()
        {
            giveUpButton.onClick.RemoveListener(GiveUp);
            feedbackButton.onClick.RemoveListener(ReportBug);
        }

        private void ReportBug()
        {
            feedbackButtonPressed.Invoke();
        }

        private void GiveUp()
        {
            giveUpPressed.Invoke();
            SceneManager.LoadScene(startScene);
        }
    }
}