using System;
using System.Collections.Generic;
using Features.Towns.Development.Config.Milestones;

namespace Features.Towns.Development.Logic.Milestones
{
    public sealed class MilestoneModel
    {
        public event Action<MilestoneUpgradeData> UpgradeAdded, UpgradeRemoved;

        public List<MilestoneUpgradeData> TownUpgrades { get; } = new();

        public void AddUpgrade(MilestoneUpgradeData data)
        {
            TownUpgrades.Add(data);
            UpgradeAdded?.Invoke(data);
        }

        public void RemoveUpgrade(MilestoneUpgradeData data)
        {
            TownUpgrades.Remove(data);
            UpgradeRemoved?.Invoke(data);
        }
    }
}