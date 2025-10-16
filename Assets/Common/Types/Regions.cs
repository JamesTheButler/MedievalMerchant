using System;

namespace Common.Types
{
    [Flags]
    public enum Regions
    {
        Forest = 1 << Region.Forest,
        Ocean = 1 << Region.Ocean,
        Fields = 1 << Region.Fields,
        Mountains = 1 << Region.Mountains,
    }
}