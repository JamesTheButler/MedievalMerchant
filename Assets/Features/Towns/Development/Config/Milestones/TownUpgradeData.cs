using UnityEngine;

namespace Features.Towns.Development.Config.Milestones
{
    public abstract class TownUpgradeData : ScriptableObject
    {
        public abstract string Description { get; }
    }
}