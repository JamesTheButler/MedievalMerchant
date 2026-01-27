using System;
using Common.Infrastructure;
using Features.Towns.Flags.Logic;

namespace Features.Towns.Initialization
{
    [Serializable]
    public sealed class RandomIdentityInitData : IdentityInitData
    {
        public override void Initialize(Town town)
        {
            var townResources = ResourceManager.Instance.TownResources;
            town.Name = townResources.NameGenerators[town.MainRegion].GenerateName();
        }
    }
}