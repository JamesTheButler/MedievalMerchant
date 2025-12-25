using System.Collections.Generic;
using Common.Infrastructure;
using Common.Types;
using Features.Towns.Development.Config;
using Features.Towns.Development.Config.Milestones;
using UnityEngine;

namespace Features.Towns.Development.Logic.Milestones
{
    /// <summary>
    /// Observes town development and activates/deactivates milestones.
    /// </summary>
    public sealed class DevelopmentMilestoneSystem : ISystem
    {
        private sealed record UpgradeTime(Tier Tier, float DevelopmentScore);

        private readonly Dictionary<UpgradeTime, List<MilestoneUpgradeData>> _milestones = new();
        private readonly Town _town;
        private readonly TownDevelopmentConfig _developmentConfig;
        private readonly MilestoneModel _milestoneModel;

        private float _previousScore;
        private Tier _currentTier;
        private DevelopmentMilestoneDataSet _milestoneSet;

        public DevelopmentMilestoneSystem(Town town)
        {
            _town = town;

            _developmentConfig = ConfigurationManager.Configurations.TownDevelopmentConfig;
            _milestoneModel = town.Milestones;
        }

        public void Initialize()
        {
            _town.Tier.Observe(OnTierChanged);
            _town.DevelopmentManager.DevelopmentScore.Observe(OnDevelopmentChanged);
        }

        public void CleanUp()
        {
            _town.Tier.StopObserving(OnTierChanged);
            _town.DevelopmentManager.DevelopmentScore.StopObserving(OnDevelopmentChanged);
        }

        private void OnTierChanged(Tier tier)
        {
            _currentTier = tier;
            _milestoneSet = _developmentConfig.Milestones[tier];
            _previousScore = 0;
        }

        private void OnDevelopmentChanged(float score)
        {
            foreach (var (thresholdPercent, milestoneData) in _milestoneSet.MilestoneData)
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
                        _milestoneModel.AddUpgrade(upgrade);
                        _milestones.TryAdd(upgradeTime, new List<MilestoneUpgradeData>());
                        _milestones[upgradeTime].Add(upgrade);
                    }
                }

                // milestone re-locked
                else if (_previousScore >= thresholdScore && score < thresholdScore)
                {
                    if (!_milestones.TryGetValue(upgradeTime, out var lockedMilestones))
                        return;
                    
                    foreach (var upgradeData in lockedMilestones)
                    {
                        _milestoneModel.RemoveUpgrade(upgradeData);
                    }

                    lockedMilestones.Clear();
                    _milestones.Remove(upgradeTime);
                }
            }

            _previousScore = score;
        }
    }
}