using Common.Infrastructure;
using Features.Ticking;
using Features.Ticking.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Common.UI
{
    public class GameOverUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private GameOverUI gameOverUi;

        private GameSpeedModel _gameSpeedModel;

        private void Start()
        {
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            gameOverUi.Hide();
        }

        public void Win()
        {
            _gameSpeedModel.Pause();
            gameOverUi.Show(true);
        }

        public void Lose()
        {
            _gameSpeedModel.Pause();
            gameOverUi.Show(false);
        }
    }
}