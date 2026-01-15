using Common.Infrastructure.Gameplay;
using Features.Levels.Conditions.Model;
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
        private LevelConditions _conditions;

        private void Start()
        {
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _conditions = GameplayContext.Instance.Model.Conditions;
            _conditions.LevelWon += OnWin;
            _conditions.LevelLost += OnLoss;

            gameOverUi.Hide();
        }

        private void OnWin()
        {
            _gameSpeedModel.Pause();
            gameOverUi.ShowWin();
        }

        private void OnLoss(ILossCondition lossCondition)
        {
            _gameSpeedModel.Pause();
            gameOverUi.ShowLoss(lossCondition);
        }
    }
}