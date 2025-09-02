using System;

namespace Data
{
    [Flags]
    public enum PlayerUpgrades
    {
        None = 1 << 0,
        Tier1Slots3 = 1 << 1,
        Tier1Slots6 = 1 << 2,
        Tier1Slots9 = 1 << 3,
        Tier1Slots12 = 1 << 4,
        Tier2Slots3 = 1 << 5,
        Tier2Slots6 = 1 << 6,
        Tier3Slots3 = 1 << 7,
        Tier3Slots6 = 1 << 8,
    }
}