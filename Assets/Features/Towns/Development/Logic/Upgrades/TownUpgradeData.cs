using UnityEngine;

namespace Features.Towns.Development.Logic.Upgrades
{
    public abstract class TownUpgradeData : ScriptableObject
    {
        public abstract string Description { get; }
    }
}