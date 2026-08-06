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
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

namespace Features.GameOver
{
    public sealed class GameOverUI : DynamicPanel
    {
        [SerializeField, Scene]
        private string startScene;

        [SerializeField, Required]
        private TMP_Text titleText, messageText, failureText;

        [SerializeField, Required]
        private GameObject statValueItem;

        [SerializeField, Required]
        private Transform statValueContainer;

        [SerializeField]
        private LocalizedString levelWonTitle,
            levelLosTitle,
            levelWonDescription,
            levelLostDescription,
            favoriteGoodString;

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

            titleText.text = isWon
                ? levelWonTitle.GetLocalizedString().WithStyle(Style.Good)
                : levelLosTitle.GetLocalizedString().WithStyle(Style.Bad);

            var currentLevel = GlobalContext.CurrentLevelInfo!;

            var levelDataObj = new
            {
                _int_LevelIndex = currentLevel.DisplayIndex,
                LevelName = currentLevel.LevelName.GetLocalizedString()
            };

            var message = isWon
                ? levelWonDescription.GetLocalizedString(levelDataObj)
                : levelLostDescription.GetLocalizedString(levelDataObj);

            messageText.text = message;
            failureText.gameObject.SetActive(!isWon);
            GenerateStatisticsText();
            Open();
        }

        private void GenerateStatisticsText()
        {
            var towns = _model.Towns.Values.ToList();
            var t2TownCount = towns.Count(town => town.Tier.Value == Tier.Tier2);
            var t3TownCount = towns.Count(town => town.Tier.Value == Tier.Tier3);
            var averageRep = towns.Average(town => town.ReputationModel.Reputation.Value);

            // we subtract towns.Count as each town starts with one producer that the player has not built
            var productionBuildingCount = towns.Sum(town => town.ProductionManager.AllProducers.Count()) - towns.Count;

            AddStatEntry(_model.DateModel.GameDate.Value.ToDisplayString());
            AddStatEntry(_model.Player.Inventory.Funds.Value.ToString("0.#"));
            AddStatEntry(t2TownCount.ToString());
            AddStatEntry(t3TownCount.ToString());
            AddStatEntry(productionBuildingCount.ToString());
            AddStatEntry(_statsModel.TotalGoodsTraded.ToString());
            AddStatEntry(_statsModel.TotalValueBought.ToString("0.#"));
            AddStatEntry(GenerateFavoriteGoodString());
            AddStatEntry(averageRep.ToString("0.#"));
        }

        private void AddStatEntry(string content)
        {
            var entry = Instantiate(statValueItem, statValueContainer);
            entry.GetComponent<TMP_Text>().text = content;
        }

        private string GenerateFavoriteGoodString()
        {
            if (_statsModel.SoldGoods.IsEmpty())
                return StateNotFoundsString;

            var favoriteGood = _statsModel.SoldGoods
                .OrderByDescending(kvPair => kvPair.Value)
                .First().Key;
            var favoriteGoodName = _goodResources.ResourceData[favoriteGood].GoodName;
            var favoriteGoodAmount = _statsModel.SoldGoods[favoriteGood];

            var favoriteGoodInfo = new
            {
                GoodName = favoriteGoodName,
                _int_Amount = favoriteGoodAmount
            };
            return favoriteGoodString.GetLocalizedString(favoriteGoodInfo);
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene(startScene);
        }
    }
}