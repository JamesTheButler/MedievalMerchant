using UnityEngine;

namespace Features.Towns.Development.Config.Milestones
{
    public abstract class MilestoneUpgradeData : ScriptableObject
    {
        public abstract string Description { get; }
    }
}