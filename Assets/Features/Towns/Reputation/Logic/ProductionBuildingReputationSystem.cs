using Common.Infrastructure;
using Common.Types;
using Features.Towns.Production.Logic;
using Features.Towns.Reputation.Data;

namespace Features.Towns.Reputation.Logic
{
    public sealed class ProductionBuildingReputationSystem : ISystem
    {
        private readonly ReputationModel _reputationModel;
        private readonly ProductionManager _productionManager;
        private ReputationConfig _reputationConfig;

        public ProductionBuildingReputationSystem(Town town)
        {
            _reputationModel = town.ReputationModel;
            _productionManager = town.ProductionManager;
        }

        public void Initialize()
        {
            _reputationConfig = ConfigurationManager.Configurations.ReputationConfig;
            _productionManager.ProductionAdded.Observe(OnProductionBuildingBuilt);
        }

        public void CleanUp()
        {
            _productionManager.ProductionAdded.StopObserving(OnProductionBuildingBuilt);
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
            _reputationModel.UpdateReputation(repChange, message);
        }
    }
}