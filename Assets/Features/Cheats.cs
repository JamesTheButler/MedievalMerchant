using System;
using Common;
using Common.Types;
using Features.Player;
using Features.Player.Caravan.Config;
using Infrastructure;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Features
{
    public sealed class Cheats : MonoBehaviour
    {
        private readonly Lazy<GameplayModel> _model = new(() => GameplayContext.Model);
        private readonly Lazy<PlayerModel> _playerModel = new(() => GameplayContext.Model.Player);

        public void AddFunds(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            _playerModel.Value.Inventory.AddFunds(5000);
        }

        /// <summary>
        /// Upgrade each cart 0 - 2 times.
        /// </summary>
        public void RandomPlayerUpgrade(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

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
        public void CompletePlayerUpgrade(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

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
        public void RandomTownUpgrade(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

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
        public void CompleteTownUpgrade(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

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
    }
}