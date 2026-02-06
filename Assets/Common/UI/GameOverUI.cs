using System.Linq;
using System.Text;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Global;
using Common.Types;
using Common.UI.Elements.Panels;
using Common.UI.Utility;
using Common.Utility;
using Features.Goods.Config;
using Features.Levels.Conditions.Model;
using Features.Stats;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Common.UI
{
    public sealed class GameOverUI : DynamicPanel
    {
        [SerializeField, Scene]
        private string startScene;

        [SerializeField, Required]
        private TMP_Text titleText, messageText, failureText, dynamicStatsText;

        private const string StateNotFoundsString = "-";

        private GoodResources _goodResources;
        private GameplayModel _model;
        private StatsModel _statsModel;

        public override void Initialize()
        {
            _goodResources = ResourceManager.Instance.GoodResources;
            _model = GameplayContext.Instance.Model;
            _statsModel = _model.Stats;
        }

        public void ShowWin()
        {
            Show(true);
        }

        public void ShowLoss(ILossCondition lossCondition)
        {
            failureText.text = lossCondition.GameOverMessage.WithStyle(Style.Bad);
            Show(false);
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }

        private void Show(bool isWon)
        {
            var playerInput = FindAnyObjectByType<PlayerInput>();
            playerInput.enabled = false;

            titleText.text = isWon ? "Level Finished!".WithStyle(Style.Good) : "Game Over!".WithStyle(Style.Bad);

            var currentLevel = GlobalContext.CurrentLevelInfo!;
            var currentLevelString = $"{currentLevel.LevelNumberText}: {currentLevel.LevelName}"
                .WithStyle(Style.Subtitle);

            var message = isWon
                ? $"Congratulations! You successfully completed {currentLevelString}"
                : $"You failed to complete {currentLevelString}";

            messageText.text = message;
            failureText.gameObject.SetActive(!isWon);
            var statisticsText = GenerateStatisticsText();
            dynamicStatsText.text = statisticsText;
            Debug.Log($"Game Over Stats: {dynamicStatsText}");
            Open();
        }

        private string GenerateStatisticsText()
        {
            var towns = _model.Towns.Values.ToList();
            var t2TownCount = towns.Count(town => town.Tier.Value == Tier.Tier2);
            var t3TownCount = towns.Count(town => town.Tier.Value == Tier.Tier3);
            var averageRep = towns.Average(town => town.ReputationModel.Reputation.Value);
            var productionBuildingCount =
                towns.Sum(town => town.ProductionManager.AllProducers.Count())
                - towns.Count; // each town starts with one producer

            var favoriteGoodString = GenerateFavoriteGoodString();

            var stringBuilder = new StringBuilder();
            stringBuilder
                .AppendLine(_model.DateModel.GameDate.Value.ToDisplayString())
                .AppendLine(_model.Player.Inventory.Funds.Value.ToString("0.#"))
                .AppendLine(t2TownCount.ToString())
                .AppendLine(t3TownCount.ToString())
                .AppendLine(productionBuildingCount.ToString())
                .AppendLine(_statsModel.TotalGoodsTraded.ToString())
                .AppendLine(_statsModel.TotalValueBought.ToString("0.#"))
                .AppendLine(favoriteGoodString)
                .AppendLine(averageRep.ToString("0.#"));

            return stringBuilder.ToString();
        }

        private string GenerateFavoriteGoodString()
        {
            if (_statsModel.TradedGoods.IsEmpty())
                return StateNotFoundsString;

            var favoriteGood = _statsModel.TradedGoods.Max(kvPair => kvPair.Key);
            var favoriteGoodName = _goodResources.ResourceData[favoriteGood].GoodName;
            var favoriteGoodAmount = _statsModel.TradedGoods[favoriteGood];
            var favoriteGoodString = $"{favoriteGoodName} (traded {favoriteGoodAmount} times)";
            return favoriteGoodString;
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(startScene);
        }
    }
}