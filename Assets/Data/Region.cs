using System;

namespace Data
{
    [Flags]
    public enum Regions
    {
        Forest = 1 << 0,
        Ocean = 1 << 1,
        Fields = 1 << 2,
        Mountains = 1 << 3,
    }
}