using System;
using Common.Infrastructure;
using Features.Player.Logic;

namespace Features.Towns.Missions.Results
{
    public sealed class MissionResultHandler
    {
        private readonly Town _town;
        private readonly Lazy<PlayerModel> _player = new(() => GameplayContext.Instance.Model.Player);

        public MissionResultHandler(Town town)
        {
            _town = town;
        }

        public void Handle(IMissionResult result)
        {
            switch (result)
            {
                case TradeMissionPenalty penalty:
                    _town.ReputationManager.ApplyMissionReward(penalty.ReputationPenalty);
                    _town.DevelopmentManager.AddDevelopmentChange(penalty.GrowthPenalty);
                    break;
                case TradeMissionReward reward:
                    _player.Value.Inventory.AddFunds(reward.Coin);
                    _town.ReputationManager.ApplyMissionPenalty(reward.Reputation);
                    _town.DevelopmentManager.AddDevelopmentChange(reward.Growth);
                    break;
                case UpgradeMissionPenalty penalty:
                    _town.ReputationManager.ApplyMissionPenalty(penalty.ReputationPenalty);
                    _town.DevelopmentManager.AddDevelopmentChange(penalty.GrowthPenalty);
                    break;
                case UpgradeMissionReward reward:
                    _town.DevelopmentManager.Upgrade();
                    _town.ReputationManager.ApplyMissionReward(reward.ReputationReward);
                    break;
            }
        }
    }
}