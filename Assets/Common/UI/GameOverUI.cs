using System.Linq;
using System.Text;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Elements;
using Common.UI.Utility;
using Features.Goods.Config;
using Features.Stats;
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

        private GoodsResources _goodsResources;
        private GameplayModel _model;
        private StatsModel _statsModel;

        public override void Initialize()
        {
            _goodsResources = ResourceManager.Instance.GoodsResources;
            _model = GameplayContext.Instance.Model;
            _statsModel = _model.Stats;
        }

        public void Show(bool isWon)
        {
            gameObject.SetActive(true);
            titleText.text = isWon ? "Level Finished!".WithStyle(Style.Good) : "Game Over!".WithStyle(Style.Bad);

            var currentLevel = GlobalContext.CurrentLevelInfo!;
            var currentLevelString = $"{currentLevel.LevelNumberText}: {currentLevel.LevelName}";
            var message = isWon
                ? $"Congratulations! You successfully completed {currentLevelString}!"
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
            var towns = _model.Towns.Values.ToList();
            var t2TownCount = towns.Count(town => town.Tier.Value == Tier.Tier2);
            var t3TownCount = towns.Count(town => town.Tier.Value == Tier.Tier3);
            var averageRep = towns.Average(town => town.ReputationManager.Reputation);
            var productionBuildingCount =
                towns.Sum(town => town.ProductionManager.AllProducers.Count())
                - towns.Count; // each town starts with one producer

            var favoriteGood = _statsModel.TradedGoods.Max(kvPair => kvPair.Key);
            var favoriteGoodName = _goodsResources.ResourceData[favoriteGood].GoodName;
            var favoriteGoodAmount = _statsModel.TradedGoods[favoriteGood];

            var stringBuilder = new StringBuilder();
            stringBuilder
                .AppendLine(_model.Date.ToDisplayString())
                .AppendLine(_model.Player.Inventory.Funds.Value.ToString("0.#"))
                .AppendLine(t2TownCount.ToString())
                .AppendLine(t3TownCount.ToString())
                .AppendLine(productionBuildingCount.ToString())
                .AppendLine(_statsModel.TotalGoodsTraded.ToString())
                .AppendLine(_statsModel.TotalValueBought.ToString("0.#"))
                .AppendLine($"{favoriteGoodName} (traded {favoriteGoodAmount} times)")
                .AppendLine(averageRep.ToString("0.#"));

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