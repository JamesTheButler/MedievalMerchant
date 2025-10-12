using System;
using System.Collections.Generic;
using Data.Configuration;
using Data.Towns.Development.Config;
using UnityEngine;

namespace Data.Towns.Development.Logic.Upgrades
{
    public sealed class TownUpgradeManager
    {
        public float PriceModifiers { get; private set; }
        public float ProductionModifiers { get; private set; }
        public float FundsModifiers { get; private set; }

        private sealed record UpgradeTime(Tier Tier, float DevelopmentScore);

        private readonly Dictionary<UpgradeTime, TownUpgradeData[]> _upgrades = new();
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
            foreach (var (threshold, milestoneData) in _milestones.MilestoneData)
            {
                var upgrades = milestoneData.Upgrades;
                if (upgrades is not { Length: > 0 })
                {
                    Debug.LogError($"upgrades were null/empty for Tier {_currentTier} at score {threshold}.");
                    continue;
                }

                // upgrade unlocked
                if (_previousScore < threshold && score >= threshold)
                {
                    _upgrades[new UpgradeTime(_currentTier, threshold)] = upgrades;
                    foreach (var upgrade in upgrades)
                    {
                        ApplyUpgrade(upgrade);
                    }
                }
                // upgrade re-locked
                else if (_previousScore >= threshold && score < threshold)
                {
                    _upgrades.Remove(new UpgradeTime(_currentTier, threshold));
                    foreach (var upgrade in upgrades)
                    {
                        RemoveUpgrade(upgrade);
                    }
                }
            }

            _previousScore = score;
        }

        private void ApplyUpgrade(TownUpgradeData upgrade)
        {
            Debug.LogWarning($"applying {upgrade.name}");
            switch (upgrade)
            {
                case FundsBoostUpgradeData fundsBoostUpgradeData:
                    FundsModifiers += fundsBoostUpgradeData.FundsBoost;
                    break;
                case PriceBoostUpgradeData priceBoostUpgradeData:
                    PriceModifiers -= priceBoostUpgradeData.PriceBoostPercent;
                    break;
                case ProductionBoostUpgradeData productionBoostUpgradeData:
                    ProductionModifiers -= productionBoostUpgradeData.ProductionBoost;
                    break;
                case SelfSufficienyUpgradeData selfSufficienyUpgradeData: break;
                case DividendsUpgradeData dividendsUpgradeData: break;
                default: throw new ArgumentOutOfRangeException(nameof(upgrade));
            }
        }

        private void RemoveUpgrade(TownUpgradeData upgrade)
        {
            Debug.LogWarning($"removing {upgrade.name}");
            switch (upgrade)
            {
                case FundsBoostUpgradeData fundsBoostUpgradeData:
                    FundsModifiers -= fundsBoostUpgradeData.FundsBoost;
                    break;
                case PriceBoostUpgradeData priceBoostUpgradeData:
                    PriceModifiers -= priceBoostUpgradeData.PriceBoostPercent;
                    break;
                case ProductionBoostUpgradeData productionBoostUpgradeData:
                    ProductionModifiers -= productionBoostUpgradeData.ProductionBoost;
                    break;
                case DividendsUpgradeData dividendsUpgradeData: break;
                case SelfSufficienyUpgradeData selfSufficienyUpgradeData: break;
                default: throw new ArgumentOutOfRangeException(nameof(upgrade));
            }
        }
    }
}