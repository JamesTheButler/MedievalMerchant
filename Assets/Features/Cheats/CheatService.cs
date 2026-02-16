using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Global;
using Common.Types;
using Common.Utility;
using Features.Player.Camp.UI;
using Features.Player.Caravan.Config;
using Features.Player.Logic;
using Features.Player.UI;
using Features.Towns;
using Features.Tutorial;
using Features.Tutorial.Logic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Features.Cheats
{
    public sealed class CheatService : IService
    {
        private GameplayModel _model;
        private PlayerModel _playerModel;
        private ProgressModel _progressModel;
        private DateModel _gameDateModel;

        private Selection _selection;
        private TutorialService _tutorialService;

        private Dictionary<string, Action> _simpleCommands;
        private Dictionary<string, Action<string>> _paramCommands;

        public void Initialize()
        {
            _model = GameplayContext.Instance.Model;
            _gameDateModel = _model.DateModel;
            _playerModel = _model.Player;
            _selection = GameplayContext.Instance.Selection;
            _tutorialService = GlobalContext.Instance.Services.TutorialService;
            _progressModel = GlobalContext.Instance.Model.ProgressModel;

            _simpleCommands = new Dictionary<string, Action>
            {
                { "date", ResetDate },
                { "funds", AddFunds },
                { "win", CompleteTownUpgrade },
                { "player.upgrade.random", RandomPlayerUpgrade },
                { "player.upgrade.full", CompletePlayerUpgrade },
                { "town.upgrade", UpgradeSelectedTown },
                { "town.upgrade.full", FullyUpgradeSelectedTown },
                { "town.upgrade.random", RandomTownUpgrade },
                { "town.upgrade.all", CompleteTownUpgrade },
                { "reset.levels", ResetCompletedLevels },
                { "reset.tutorial", ResetTutorial },
                { "reset.all", ResetAllProgress },
                { "town.grow", SetTownDevelopmentTo100 },
                { "town.rep", SetTownReputationTo100 },
                { "town.reputation", SetTownReputationTo100 },
                { "glint", GlintFunds },
                { "camp", EnterCamp },
            };

            _paramCommands = new Dictionary<string, Action<string>>
            {
                { "day", SetDay },
                { "funds", AddFunds },
                { "reset.level", ResetLevelProgress },
                { "tutorial", OpenTutorial },
                { "give", GiveGoods },
                { "town.grow", AddTownDevelopment },
                { "town.rep", SetTownReputation },
                { "town.reputation", SetTownReputation },
                { "town.funds", AddTownFunds },
            };
        }

        public void CleanUp() { }

        public void HandleSimpleCheat(string command)
        {
            if (_simpleCommands.TryGetValue(command, out var simpleCommand))
            {
                try
                {
                    simpleCommand.Invoke();
                    ReportSuccess($"Cheat '{command}' executed");
                }
                catch (Exception exception)
                {
                    ReportError($"Exception while executing cheat '{command}':\n {exception}");
                }
            }
            else
            {
                ReportError($"Unknown cheat '{command}'");
            }
        }

        public void HandleParamCheat(string command, string parameter)
        {
            if (_paramCommands.TryGetValue(command, out var paramCommand))
            {
                try
                {
                    paramCommand.Invoke(parameter);
                    ReportSuccess($"Cheat '{command} {parameter}' executed");
                }
                catch (Exception exception)
                {
                    ReportError($"Exception while executing cheat '{command}':\n {exception}");
                }
            }
            else
            {
                ReportError($"Unknown cheat '{command}'");
            }
        }

        #region Cheats

        private void GlintFunds()
        {
            var playerMiniUI = Object.FindAnyObjectByType<PlayerMiniUI>();
            playerMiniUI.PlayCoinEffect();
        }

        private void EnterCamp()
        {
            var campPanel = Object.FindFirstObjectByType<CampPanelUI>(FindObjectsInactive.Include).gameObject;
            campPanel.SetActive(true);
        }

        private void AddTownDevelopment(string parameter)
        {
            var selectedTown = _selection.SelectedTown.Value;
            if (selectedTown == null)
            {
                ReportError("No town was selected.");
                return;
            }

            var devChange = int.Parse(parameter).Clamp(0, 100);
            selectedTown.DevelopmentManager.AddDevelopmentChange(devChange);
        }

        private void SetTownDevelopmentTo100()
        {
            var selectedTown = _selection.SelectedTown.Value;
            if (selectedTown == null)
            {
                ReportError("No town was selected.");
                return;
            }

            selectedTown.DevelopmentManager.AddDevelopmentChange(100);
        }

        private void SetTownReputation(string parameter)
        {
            var selectedTown = _selection.SelectedTown.Value;
            if (selectedTown == null)
            {
                ReportError("No town was selected.");
                return;
            }

            var repChange = int.Parse(parameter);
            selectedTown.ReputationModel.UpdateReputation(repChange, "You cheated!!");
        }

        private void AddTownFunds(string parameter)
        {
            var selectedTown = _selection.SelectedTown.Value;
            if (selectedTown == null)
            {
                ReportError("No town was selected.");
                return;
            }

            var fundsChange = int.Parse(parameter);
            selectedTown.Inventory.AddFunds(fundsChange);
        }

        private void SetTownReputationTo100()
        {
            var selectedTown = _selection.SelectedTown.Value;
            if (selectedTown == null)
            {
                ReportError("No town was selected.");
                return;
            }

            selectedTown.ReputationModel.UpdateReputation(200f, "You cheated!!");
        }

        private void GiveGoods(string parameter)
        {
            var playerInventory = GameplayContext.Instance.Model.Player.Inventory;
            var good = Enum.Parse<Good>(parameter, true);

            if (playerInventory.InventoryPolicy.CanAdd(good, 50).Success)
            {
                playerInventory.AddGood(good, 50);
            }
        }

        private void UpgradeSelectedTown()
        {
            var selectedTown = _selection.SelectedTown.Value;
            if (selectedTown == null)
            {
                ReportError("No town was selected.");
                return;
            }

            selectedTown.DevelopmentManager.Upgrade();
        }

        private void FullyUpgradeSelectedTown()
        {
            var selectedTown = _selection.SelectedTown.Value;
            if (selectedTown == null)
            {
                ReportError("No town was selected.");
                return;
            }

            selectedTown.DevelopmentManager.Upgrade();
            selectedTown.DevelopmentManager.Upgrade();
            selectedTown.DevelopmentManager.Upgrade();
        }

        private void ResetDate()
        {
            _gameDateModel.SetDay(1);
        }

        private void AddFunds()
        {
            _playerModel.Inventory.AddFunds(5000);
        }

        private void AddFunds(string funds)
        {
            _playerModel.Inventory.AddFunds(int.Parse(funds));
        }

        private void SetDay(string day)
        {
            _gameDateModel.SetDay(int.Parse(day));
        }

        /// <summary>
        /// Upgrade each cart 0 - 2 times.
        /// </summary>
        private void RandomPlayerUpgrade()
        {
            var caravanManager = _playerModel.CaravanManager;
            for (var cartId = 0; cartId < CaravanConfig.MaxCartCount; cartId++)
            {
                var upgradeCount = Random.Range(0, 2);
                for (var upgradeId = 0; upgradeId < upgradeCount; upgradeId++)
                {
                    caravanManager.UpgradeCart(cartId);
                }
            }
        }

        /// <summary>
        /// Upgrade each cart fully.
        /// </summary>
        private void CompletePlayerUpgrade()
        {
            var caravanManager = _playerModel.CaravanManager;
            for (var cartId = 0; cartId < CaravanConfig.MaxCartCount; cartId++)
            {
                for (var upgradeId = 0; upgradeId < CaravanConfig.MaxLevel; upgradeId++)
                {
                    caravanManager.UpgradeCart(cartId);
                }
            }
        }

        /// <summary>
        /// Upgrade each town 0 - 1 times.
        /// </summary>
        private void RandomTownUpgrade()
        {
            var towns = _model.Towns.Values;
            foreach (var town in towns)
            {
                var upgradeCount = Random.Range(0, 2);
                for (var upgradeId = 0; upgradeId < upgradeCount; upgradeId++)
                {
                    town.DevelopmentManager.Upgrade();
                }
            }
        }

        /// <summary>
        /// Upgrade each town fully.
        /// </summary>
        private void CompleteTownUpgrade()
        {
            var towns = _model.Towns.Values;
            foreach (var town in towns)
            {
                var upgradeCount = Enum.GetValues(typeof(Tier)).Length;
                for (var upgradeId = 0; upgradeId < upgradeCount; upgradeId++)
                {
                    town.DevelopmentManager.Upgrade();
                }
            }
        }

        private void ResetTutorial()
        {
            _tutorialService.ResetCompletedTopics();
        }

        private void ResetAllProgress()
        {
            _progressModel.Reset();
            ResetTutorial();
        }

        private void ResetCompletedLevels()
        {
            _progressModel.ResetCompletedLevels();
        }

        private void ResetLevelProgress(string levelIndex)
        {
            _progressModel.ResetCompletedLevel(int.Parse(levelIndex));
        }

        private void OpenTutorial(string topic)
        {
            var tutorialTopic = Enum.Parse<TutorialTopic>(topic, true);
            _tutorialService.OpenTutorial(tutorialTopic);
        }

        #endregion

        #region Utilities

        private static void ReportSuccess(string message)
        {
            Debug.Log(message);
        }

        private void ReportError(string message)
        {
            Debug.LogError($"{message}\nAvailable cheats: {string.Join(",", _simpleCommands.Keys)}");
        }

        public void HandleInvalidInput(string input)
        {
            ReportError($"Invalid cheat '{input}' has been entered.");
        }

        #endregion
    }
}