using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Modifiable;
using Features.Goods.Selector;
using Features.Player.Logic;
using Features.Towns.Development.Config.Milestones;
using Features.Trade;
using UnityEngine;

namespace Features.Towns.Development.Logic.Milestones
{
    /// <summary>
    /// Observes milestones and applies them to alter game logic.
    /// </summary>
    public sealed class MilestoneModifierSystem : ISystem
    {
        private readonly Town _town;
        private readonly HashSet<MilestoneUpgradeData> _activeUpgrades = new();
        private readonly Dictionary<MilestoneUpgradeData, IModifier> _modifiers = new();

        private readonly Dictionary<PriceBoostUpgradeData, Tuple<MilestonePriceBoostModifier, MilestonePriceBoostModifier>>
            _priceModifiers = new();

        private GameplayModel _model;
        private PlayerModel _player;

        public MilestoneModifierSystem(Town town)
        {
            _town = town;
        }

        public void Initialize()
        {
            _model = GameplayContext.Instance.Model;
            _player = _model.Player;
            _town.Milestones.UpgradeAdded += OnMilestoneModifierAdded;
            _town.Milestones.UpgradeRemoved += OnMilestoneModifierRemoved;
        }

        public void CleanUp()
        {
            _town.Milestones.UpgradeAdded -= OnMilestoneModifierAdded;
            _town.Milestones.UpgradeRemoved -= OnMilestoneModifierRemoved;
        }

        private void OnMilestoneModifierAdded(MilestoneUpgradeData upgrade)
        {
            if (!_activeUpgrades.Add(upgrade))
            {
                Debug.LogWarning($"{upgrade.GetType().Name} was already granted to {_town.Name} - skipping.");
                return;
            }

            switch (upgrade)
            {
                case FundsBoostUpgradeData upgradeData:
                    var fundsBoostModifier = new MilestoneFundsBoostModifier(upgradeData.FundsBoost);
                    _town.FundsChange.AddModifier(fundsBoostModifier);
                    _modifiers.Add(upgrade, fundsBoostModifier);
                    break;

                case PriceBoostUpgradeData priceUpgradeData:
                    var sellPriceBoostModifier = new MilestonePriceBoostModifier(priceUpgradeData.PriceBoostPercent);
                    var buyPriceBoostModifier = new MilestonePriceBoostModifier(-priceUpgradeData.PriceBoostPercent);
                    _priceModifiers.Add(priceUpgradeData,
                        new Tuple<MilestonePriceBoostModifier, MilestonePriceBoostModifier>(
                            buyPriceBoostModifier,
                            sellPriceBoostModifier));
                    _town.PriceManager.AddModifier(sellPriceBoostModifier, IGoodSelector.All, TradeType.Sell);
                    _town.PriceManager.AddModifier(buyPriceBoostModifier, IGoodSelector.All, TradeType.Buy);
                    break;

                case ProductionBoostUpgradeData upgradeData:
                    var prodBoostModifier = new MilestoneProductionBoostModifier(upgradeData.ProductionBoost);
                    _town.ProductionManager.AddModifier(prodBoostModifier, IGoodSelector.All);
                    _modifiers.Add(upgrade, prodBoostModifier);
                    break;

                case DividendsUpgradeData upgradeData:
                    var dividendsModifier = new DividendsFundsModifier(upgradeData.DividendsPercentage, _town);
                    _player.FundsChange.AddModifier(dividendsModifier);
                    _modifiers.Add(upgrade, dividendsModifier);
                    break;

                case SelfSufficienyUpgradeData:
                    _town.DevelopmentManager.LockDegrowth(true);
                    break;

                default:
                    Debug.LogError($"Failed to apply unhandled upgrade {upgrade.GetType().Name}.");
                    break;
            }
        }

        private void OnMilestoneModifierRemoved(MilestoneUpgradeData upgrade)
        {
            if (!_activeUpgrades.Remove(upgrade))
                return;

            switch (upgrade)
            {
                case FundsBoostUpgradeData upgradeData:
                    _town.FundsChange.RemoveModifier(_modifiers[upgradeData]);
                    _modifiers.Remove(upgradeData);
                    break;

                case PriceBoostUpgradeData upgradeData:
                    var priceModifiers = _priceModifiers[upgradeData];
                    _town.PriceManager.RemoveModifier(priceModifiers.Item1, TradeType.Buy);
                    _town.PriceManager.RemoveModifier(priceModifiers.Item2, TradeType.Sell);
                    _priceModifiers.Remove(upgradeData);
                    break;

                case ProductionBoostUpgradeData upgradeData:
                    _town.ProductionManager.RemoveModifier(_modifiers[upgradeData], IGoodSelector.All);
                    _modifiers.Remove(upgradeData);
                    break;

                case DividendsUpgradeData upgradeData:
                    _player.FundsChange.RemoveModifier(_modifiers[upgradeData]);
                    _modifiers.Remove(upgradeData);
                    break;

                case SelfSufficienyUpgradeData:
                    _town.DevelopmentManager.LockDegrowth(false);
                    break;

                default:
                    Debug.LogError($"Failed to apply unhandled upgrade {upgrade.GetType().Name}.");
                    break;
            }
        }
    }
}