using Features.Ticking;
using Infrastructure;
using NaughtyAttributes;
using UnityEngine;

namespace UI
{
    public class GameOverUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private GameOverUI gameOverUi;

        private TickingService _tickingService;

        private void Start()
        {
            _tickingService = GameplayContext.Instance.Services.TickingService;
            gameOverUi.Hide();
        }

        public void Win()
        {
            _tickingService.Pause();
            gameOverUi.Show(true);
        }

        public void Lose()
        {
            _tickingService.Pause();
            gameOverUi.Show(false);
        }
    }
}