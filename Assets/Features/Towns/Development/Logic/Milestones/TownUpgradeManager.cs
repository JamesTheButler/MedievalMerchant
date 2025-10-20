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
        public sealed record UpgradeTime(Tier Tier, float DevelopmentScore);

        public event Action<IModifier> MilestoneModifierAdded;
        public event Action<IModifier> MilestoneModifierRemoved;

        public List<IModifier> MilestoneModifiers { get; } = new();

        private readonly Dictionary<UpgradeTime, List<IModifier>> _milestoneModifiers = new();
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

        public void Clear()
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

                // milestone unlocked
                if (_previousScore < thresholdScore && score >= thresholdScore)
                {
                    foreach (var upgrade in upgrades)
                    {
                        ApplyUpgrade(upgradeTime, upgrade);
                    }
                }

                // milestone re-locked
                else if (_previousScore >= thresholdScore && score < thresholdScore)
                {
                    RevertMilestone(upgradeTime);
                }
            }

            _previousScore = score;
        }

        private void RevertMilestone(UpgradeTime upgradeTime)
        {
            if (!_milestoneModifiers.TryGetValue(upgradeTime, out var modifiers))
                return;
            foreach (var modifier in modifiers)
            {
                MilestoneModifierRemoved?.Invoke(modifier);
                MilestoneModifiers.Remove(modifier);
            }

            modifiers.Clear();
        }

        private void ApplyUpgrade(UpgradeTime upgradeTime, TownUpgradeData upgrade)
        {
            Debug.LogWarning($"applying {upgrade.name}");
            switch (upgrade)
            {
                case FundsBoostUpgradeData upgradeData:
                    var fundsBoostModifier = new MilestoneFundsBoostModifier(upgradeData.FundsBoost, upgradeTime);
                    AddModifier(fundsBoostModifier, upgradeTime);
                    break;

                case PriceBoostUpgradeData upgradeData:
                    var priceBoostModifier =
                        new MilestonePriceBoostModifier(upgradeData.PriceBoostPercent, upgradeTime);
                    AddModifier(priceBoostModifier, upgradeTime);
                    break;

                case ProductionBoostUpgradeData upgradeData:
                    var prodBoostModifier =
                        new MilestoneProductionBoostModifier(upgradeData.ProductionBoost, upgradeTime);
                    AddModifier(prodBoostModifier, upgradeTime);
                    break;

                case DividendsUpgradeData upgradeData:
                    var dividendsModifier =
                        new DividendsFundsModifier(upgradeData.DividendsPercentage, upgradeTime, _town);
                    AddModifier(dividendsModifier, upgradeTime);
                    break;

                default:
                    Debug.LogError($"Failed to apply unhandled upgrade {upgrade.name} at {upgradeTime}");
                    break;
            }
        }

        private void AddModifier(IModifier modifier, UpgradeTime upgradeTime)
        {
            MilestoneModifiers.Add(modifier);
            _milestoneModifiers.TryAdd(upgradeTime, new List<IModifier>());
            _milestoneModifiers[upgradeTime].Add(modifier);
            MilestoneModifierAdded?.Invoke(modifier);
        }
    }
}