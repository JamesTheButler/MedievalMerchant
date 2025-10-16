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

        [SerializeField, Required]
        private Button giveUpButton, bugReportButton;

        private void Start()
        {
            giveUpButton.onClick.AddListener(GiveUp);
            bugReportButton.onClick.AddListener(ReportBug);
        }


        private void OnDestroy()
        {
            giveUpButton.onClick.RemoveListener(GiveUp);
            bugReportButton.onClick.RemoveListener(ReportBug);
        }

        private void ReportBug()
        {
            Debug.LogError("Bug Report Feature is not implemented.");
        }

        private void GiveUp()
        {
            giveUpPressed.Invoke();
            SceneManager.LoadScene(startScene);
        }
    }
}