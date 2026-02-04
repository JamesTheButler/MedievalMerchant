using System;

namespace Features.Levels
{
    [Flags]
    public enum LevelFeatureFlags
    {
        None = 0,
        Haggling = 1 << 0,
        Retinue = 1 << 1,
        Bandits = 1 << 2,
        Tutorial = 1 << 3,
    }
}