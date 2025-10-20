using System;
using System.Linq;
using Common;
using Features.Towns.Development.Config.Milestones;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Towns.Development.Config
{
    [Serializable]
    public sealed class DevelopmentMilestoneData
    {
        [field: SerializeField, Required, ShowAssetPreview(32, 32)]
        public Sprite Icon { get; private set; }

        [field: SerializeField]
        public TownUpgradeData[] Upgrades { get; private set; }

        public string Description => GenerateDescription();

        private string GenerateDescription()
        {
            return Upgrades.Length == 1
                ? Upgrades.First().Description
                : Upgrades.AggregateString(upgrade => $"- {upgrade.Description}\n");
        }
    }
}