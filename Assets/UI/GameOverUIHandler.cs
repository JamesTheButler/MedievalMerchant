using NaughtyAttributes;
using UnityEngine;

namespace UI
{
    public class GameOverUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private GameOverUI gameOverUi;

        private void Start()
        {
            gameOverUi.Hide();
        }

        public void Win()
        {
            gameOverUi.Show(true);
        }

        public void Lose()
        {
            gameOverUi.Show(false);
        }
    }
}