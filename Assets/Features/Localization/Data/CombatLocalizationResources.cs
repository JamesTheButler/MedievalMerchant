using System;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class CombatLocalizationResources
    {
        public string UnitLossOutOf(int maxUnitCount)
        {
            return $"of {maxUnitCount}";
        }
    }
}