using System.Text;
using Common.Infrastructure;
using Common.UI.Elements;
using Common.UI.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.UI
{
    public class GameOverUI : InitializableUI
    {
        [SerializeField, Scene]
        private string startScene;

        [SerializeField, Required]
        private TMP_Text titleText, messageText, failureText, dynamicStatsText;

        private GameplayModel _model;

        public override void Initialize()
        {
            _model = GameplayContext.Instance.Model;
        }

        public void Show(bool isWon)
        {
            gameObject.SetActive(true);
            titleText.text = isWon ? "Level Finished!".WithStyle(Style.Good) : "Game Over!".WithStyle(Style.Bad);

            var currentLevel = GlobalContext.CurrentLevelInfo!;
            var currentLevelString = $"{currentLevel.LevelNumberText}:{currentLevel.LevelName}";
            var message = isWon
                ? $"Congratulation! You successfully completed {currentLevelString}!"
                : $"You failed to complete {currentLevelString}!";

            messageText.text = message;

            failureText.gameObject.SetActive(false);
            /*
            failureText.gameObject.SetActive(!isWon);
            failureText.text = "You lost because... TBD";
            */
            dynamicStatsText.text = GenerateStatisticsText();
        }

        private string GenerateStatisticsText()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder
                .AppendLine(_model.Date.ToDisplayString()) // date
                .AppendLine(_model.Player.Inventory.Funds.Value.ToString("0.#")) // coins
                .AppendLine("-") // t2 towns
                .AppendLine("-") // t3 towns
                .AppendLine("-") // producers built
                .AppendLine("-") // goods traded
                .AppendLine("-") // coins earned
                .AppendLine("-") // final reputation
                ;

            return stringBuilder.ToString();
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