using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Types;
using Features.Player;
using Features.Player.Caravan.Config;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Features
{
    public sealed class CheatHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject cheatUI;

        [SerializeField, Required]
        private TMP_InputField cheatInput;

        private readonly Lazy<GameplayModel> _model = new(() => GameplayContext.Instance.Model);
        private readonly Lazy<PlayerModel> _playerModel = new(() => GameplayContext.Instance.Model.Player);

        private Dictionary<string, Action> _simpleCommands;
        private Dictionary<string, Action<string>> _paramCommands;

        private void Start()
        {
            _simpleCommands = new Dictionary<string, Action>
            {
                { "funds", AddFunds },
                { "win", CompleteTownUpgrade },
                { "player.upgrade.random", RandomPlayerUpgrade },
                { "player.upgrade.full", CompletePlayerUpgrade },
                { "town.upgrade.random", RandomTownUpgrade },
                { "town.upgrade.full", CompleteTownUpgrade },
                { "reset", ResetAllProgress },
                { "reset.progress", ResetCompletedLevels },
            };

            _paramCommands = new Dictionary<string, Action<string>>
            {
                { "funds", AddFunds },
                { "reset.level", ResetLevelProgress },
            };

            cheatUI.SetActive(false);
        }

        public void Toggle(InputAction.CallbackContext context)
        {
            if (!enabled || !context.performed)
                return;

            var isEnabled = !cheatUI.activeSelf;

            cheatUI.SetActive(isEnabled);

            cheatInput.text = string.Empty;
            if (isEnabled)
            {
                EventSystem.current.SetSelectedGameObject(cheatUI);
                cheatInput.ActivateInputField();
                cheatInput.Select();
            }
        }

        public void ConfirmInput(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            CheatInputConfirmed(cheatInput.text);
        }

        public void Cancel(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            Clear();
        }

        private void CheatInputConfirmed(string cheat)
        {
            if (!enabled) return;

            ParseCheat(cheat.ToLowerInvariant());
            Clear();
        }

        private void Clear()
        {
            cheatInput.text = string.Empty;
            cheatUI.SetActive(false);
        }

        private void ParseCheat(string cheat)
        {
            var split = cheat.Split(" ");
            switch (split.Length)
            {
                case 1:
                    HandleSimpleCheat(split[0]);
                    break;
                case 2:
                    HandleParamCheat(split[0], split[1]); break;
                default:
                    ReportError("Cheats must have 0 or 1 params.");
                    break;
            }
        }

        private void HandleSimpleCheat(string command)
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
                    ReportError($"Cheat failed to execute: {exception}");
                }
            }
            else
            {
                ReportError($"Unknown cheat '{command}'");
            }
        }

        private void HandleParamCheat(string command, string parameter)
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
                    ReportError($"Cheat failed to execute: {exception}");
                }
            }
            else
            {
                ReportError($"Unknown cheat '{command}'");
            }
        }

        private void AddFunds()
        {
            _playerModel.Value.Inventory.AddFunds(5000);
        }

        private void AddFunds(string funds)
        {
            _playerModel.Value.Inventory.AddFunds(int.Parse(funds));
        }

        /// <summary>
        /// Upgrade each cart 0 - 2 times.
        /// </summary>
        private void RandomPlayerUpgrade()
        {
            var caravanManager = _playerModel.Value.CaravanManager;
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
            var caravanManager = _playerModel.Value.CaravanManager;
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
            var towns = _model.Value.Towns.Values;
            foreach (var town in towns)
            {
                var upgradeCount = Random.Range(0, 2);
                for (var upgradeId = 0; upgradeId < upgradeCount; upgradeId++)
                {
                    town.Upgrade();
                }
            }
        }

        /// <summary>
        /// Upgrade each town fully.
        /// </summary>
        private void CompleteTownUpgrade()
        {
            var towns = _model.Value.Towns.Values;
            foreach (var town in towns)
            {
                var upgradeCount = Enum.GetValues(typeof(Tier)).Length;
                for (var upgradeId = 0; upgradeId < upgradeCount; upgradeId++)
                {
                    town.Upgrade();
                }
            }
        }

        private void ResetAllProgress()
        {
            GlobalContext.Instance.ProgressModel.Reset();
        }

        private void ResetCompletedLevels()
        {
            GlobalContext.Instance.ProgressModel.ResetCompletedLevels();
        }

        private void ResetLevelProgress(string levelIndex)
        {
            GlobalContext.Instance.ProgressModel.ResetCompletedLevel(int.Parse(levelIndex));
        }

        private static void ReportSuccess(string message)
        {
            Debug.Log(message);
        }

        private static void ReportError(string message)
        {
            Debug.LogError(message);
        }
    }
}