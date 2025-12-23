using System;

namespace Common.Types
{
    [Flags]
    public enum Regions
    {
        None = 0,

        Forest = 1 << Region.Forest,
        Ocean = 1 << Region.Ocean,
        Fields = 1 << Region.Fields,
        Mountains = 1 << Region.Mountains,

        All = Forest | Ocean | Fields | Mountains
    }
}