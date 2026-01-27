using System;

namespace Features.Towns.Initialization
{
    [Serializable]
    public abstract class InitData
    {
        public abstract void Initialize(Town town);
    }
}