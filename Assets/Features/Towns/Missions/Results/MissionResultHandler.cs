using System;
using Common.Infrastructure.Gameplay;
using Features.Player.Logic;

namespace Features.Towns.Missions.Results
{
    public sealed class MissionResultHandler
    {
        private readonly Town _town;
        private readonly Lazy<PlayerModel> _player = new(() => GameplayContext.Instance.Model.Player);

        private readonly string _missionRewardRepLogMessage, _missionPenaltyRepLogMessage;

        public MissionResultHandler(Town town)
        {
            _town = town;
            _missionRewardRepLogMessage = $"You supplied {_town.Name} with the goods they wanted.";
            _missionPenaltyRepLogMessage = $"You failed to supply {_town.Name} in time.";
        }

        public void Handle(IMissionResult result)
        {
            switch (result)
            {
                case TradeMissionPenalty penalty:
                    _town.ReputationManager.UpdateReputation(penalty.ReputationPenalty, _missionPenaltyRepLogMessage);
                    _town.DevelopmentManager.AddDevelopmentChange(penalty.GrowthPenalty);
                    break;
                case TradeMissionReward reward:
                    _player.Value.Inventory.AddFunds(reward.Coin);
                    _town.ReputationManager.UpdateReputation(reward.Reputation, _missionRewardRepLogMessage);
                    _town.DevelopmentManager.AddDevelopmentChange(reward.Growth);
                    break;
                case UpgradeMissionPenalty penalty:
                    _town.ReputationManager.UpdateReputation(penalty.ReputationPenalty, _missionPenaltyRepLogMessage);
                    _town.DevelopmentManager.AddDevelopmentChange(penalty.GrowthPenalty);
                    break;
                case UpgradeMissionReward reward:
                    _town.DevelopmentManager.Upgrade();
                    _town.ReputationManager.UpdateReputation(reward.ReputationReward, _missionRewardRepLogMessage);
                    break;
            }
        }
    }
}