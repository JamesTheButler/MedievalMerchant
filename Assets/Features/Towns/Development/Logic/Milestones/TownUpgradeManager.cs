using System;
using System.Collections.Generic;
using Common;
using Common.Modifiable;
using Common.Types;
using Features.Towns.Development.Config;
using Features.Towns.Development.Config.Milestones;
using UnityEngine;

namespace Features.Towns.Development.Logic.Milestones
{
    public sealed class TownUpgradeManager
    {
        public event Action<IModifier> MilestoneModifierAdded;
        public event Action<IModifier> MilestoneModifierRemoved;

        public IEnumerable<IModifier> MilestoneModifiers => _milestoneModifiers.Values;

        public sealed record UpgradeTime(Tier Tier, float DevelopmentScore);

        private readonly Dictionary<UpgradeTime, TownUpgradeData[]> _upgradeDatas = new();
        private readonly Dictionary<UpgradeTime, IModifier> _milestoneModifiers = new();
        private readonly Town _town;
        private readonly TownDevelopmentConfig _upgradeProgressionConfig;

        private float _previousScore;
        private Tier _currentTier;
        private DevelopmentMilestoneDataSet _milestones;

        public TownUpgradeManager(Town town)
        {
            _town = town;

            _upgradeProgressionConfig = ConfigurationManager.Instance.TownDevelopmentConfig;

            _town.Tier.Observe(OnTierChanged);
            _town.DevelopmentManager.DevelopmentScore.Observe(OnDevelopmentChanged);
        }

        ~TownUpgradeManager()
        {
            _town.Tier.StopObserving(OnTierChanged);
            _town.DevelopmentManager.DevelopmentScore.StopObserving(OnDevelopmentChanged);
        }

        private void OnTierChanged(Tier tier)
        {
            _currentTier = tier;
            _milestones = _upgradeProgressionConfig.Milestones[tier];
            _previousScore = 0;
        }

        private void OnDevelopmentChanged(float score)
        {
            foreach (var (thresholdPercent, milestoneData) in _milestones.MilestoneData)
            {
                var upgrades = milestoneData.Upgrades;
                var thresholdScore = thresholdPercent * 100f;

                if (upgrades is not { Length: > 0 })
                {
                    Debug.LogError($"upgrades were null/empty for Tier {_currentTier} at score {thresholdScore}.");
                    continue;
                }

                var upgradeTime = new UpgradeTime(_currentTier, thresholdPercent);
                // upgrade unlocked
                if (_previousScore < thresholdScore && score >= thresholdScore)
                {
                    _upgradeDatas[upgradeTime] = upgrades;
                    foreach (var upgrade in upgrades)
                    {
                        ApplyUpgrade(upgradeTime, upgrade);
                    }
                }
                // upgrade re-locked
                else if (_previousScore >= thresholdScore && score < thresholdScore)
                {
                    _upgradeDatas.Remove(upgradeTime);
                    foreach (var upgrade in upgrades)
                    {
                        RemoveUpgrade(upgradeTime, upgrade);
                    }
                }
            }

            _previousScore = score;
        }

        private void ApplyUpgrade(UpgradeTime upgradeTime, TownUpgradeData upgrade)
        {
            Debug.LogWarning($"applying {upgrade.name}");
            switch (upgrade)
            {
                case FundsBoostUpgradeData upgradeData:
                    var fundsBoostModifier =
                        new MilestoneFundsBoostModifier(upgradeData.FundsBoost, upgradeTime);
                    _milestoneModifiers.Add(upgradeTime, fundsBoostModifier);
                    MilestoneModifierAdded?.Invoke(fundsBoostModifier);
                    break;

                case PriceBoostUpgradeData upgradeData:
                    var priceBoostModifier =
                        new MilestonePriceBoostModifier(upgradeData.PriceBoostPercent, upgradeTime);
                    _milestoneModifiers.Add(upgradeTime, priceBoostModifier);
                    MilestoneModifierAdded?.Invoke(priceBoostModifier);
                    break;

                case ProductionBoostUpgradeData upgradeData:
                    var prodBoostModifier =
                        new MilestoneProductionBoostModifier(upgradeData.ProductionBoost, upgradeTime);
                    _milestoneModifiers.Add(upgradeTime, prodBoostModifier);
                    MilestoneModifierAdded?.Invoke(prodBoostModifier);
                    break;

                case DividendsUpgradeData upgradeData:
                    var dividendsModifier =
                        new DividendsFundsModifier(upgradeData.DividendsPercentage, upgradeTime, _town);
                    _milestoneModifiers.Add(upgradeTime, dividendsModifier);
                    MilestoneModifierAdded?.Invoke(dividendsModifier);
                    break;

                case SelfSufficienyUpgradeData:
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(upgrade));
            }
        }

        // TODO - BUG: this doens't work, because we don't have a list of modifiers per upgrade time (like we have for datas)
        private void RemoveUpgrade(UpgradeTime upgradeTime, TownUpgradeData upgrade)
        {
            Debug.LogWarning($"removing {upgrade.name}");
            var modifier = _milestoneModifiers.GetValueOrDefault(upgradeTime);
            if (modifier is not null)
            {
                _milestoneModifiers.Remove(upgradeTime);
                MilestoneModifierRemoved?.Invoke(modifier);
            }

            switch (upgrade)
            {
                case FundsBoostUpgradeData:
                case PriceBoostUpgradeData:
                case ProductionBoostUpgradeData:
                case DividendsUpgradeData:
                case SelfSufficienyUpgradeData: break;

                default: throw new ArgumentOutOfRangeException(nameof(upgrade));
            }
        }
    }
}