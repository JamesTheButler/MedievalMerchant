using System;
using Common.Infrastructure;
using Common.Utility;

namespace Features.Towns.Initialization
{
    [Serializable]
    public sealed class RandomProductionInitData : ProductionInitData
    {
        public override void Initialize(Town town)
        {
            var startGood = town.AvailableGoods.GetRandom();
            town.ProductionManager.AddProducer(startGood, 0);

            var townConfig = ConfigurationManager.Configurations.TownConfig;
            town.Inventory.AddGood(startGood, townConfig.GetStartGoods());
        }
    }
}