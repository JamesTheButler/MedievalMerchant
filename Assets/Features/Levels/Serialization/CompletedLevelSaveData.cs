using System;
using Common.Types;

namespace Features.Levels.Serialization
{
    [Serializable]
    public sealed record CompletedLevelSaveData(Date CompletionDate)
    {
        public CompletedLevelSaveData() : this(new Date()) { }
    }
}