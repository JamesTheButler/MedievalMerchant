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
        private Button giveUpButton;

        private void Start()
        {
            giveUpButton.onClick.AddListener(GiveUp);
        }

        private void OnDestroy()
        {
            giveUpButton.onClick.RemoveListener(GiveUp);
        }

        private void GiveUp()
        {
            giveUpPressed.Invoke();
            SceneManager.LoadScene(startScene);
        }
    }
}