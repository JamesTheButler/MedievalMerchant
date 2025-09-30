using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField, Scene]
        private string startScene;
        
        [SerializeField, Required]
        private GameObject winUI, lossUi;

        public void Show(bool isWon)
        {
            gameObject.SetActive(true);

            winUI.SetActive(isWon);
            lossUi.SetActive(!isWon);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(startScene);
        }
    }
}