using Common.Infrastructure;
using Common.Types;
using Features.Towns.Production.Logic;
using Features.Towns.Reputation.Data;

namespace Features.Towns.Reputation.Logic
{
    public sealed class ProductionBuildingReputationSystem : ISystem
    {
        private readonly ReputationManager _reputationManager;
        private readonly ProductionManager _productionManager;
        private ReputationConfig _reputationConfig;

        public ProductionBuildingReputationSystem(Town town)
        {
            _reputationManager = town.ReputationManager;
            _productionManager = town.ProductionManager;
        }

        public void Initialize()
        {
            _reputationConfig = ConfigurationManager.Configurations.ReputationConfig;
            _productionManager.ProductionAdded += OnProductionBuildingBuilt;
        }

        public void CleanUp()
        {
            _productionManager.ProductionAdded -= OnProductionBuildingBuilt;
        }

        private void OnProductionBuildingBuilt(Producer producer)
        {
            var tier = producer.Tier;
            var reputationRewardData = _reputationConfig.RewardData;
            var repChange = tier switch
            {
                Tier.Tier1 => reputationRewardData.Tier1ProductionBuilding,
                Tier.Tier2 => reputationRewardData.Tier2ProductionBuilding,
                Tier.Tier3 => reputationRewardData.Tier3ProductionBuilding,
                _ => 0
            };
            var good = producer.ProducedGood;
            var message = $"Player constructed a production building ({good}) of {tier.ToDisplayString()}";
            _reputationManager.UpdateReputation(repChange, message);
        }
    }
}