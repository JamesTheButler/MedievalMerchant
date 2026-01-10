using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Goods.Config;
using Features.Towns.Production.Logic;
using Features.Towns.Reputation.Data;
using Features.Trade;
using UnityEngine;

namespace Features.Towns.Reputation.Logic
{
    public sealed class ReputationManager
    {
        private readonly Town _town;

        public Observable<float> Reputation { get; } = new();

        public IReadOnlyList<ReputationLogEntry> ReputationLog => _reputationLog;
        public IReadOnlyList<IModifier> Modifiers => _modifiers;
        public Observable<bool> IsNeglected { get; set; } = new();

        private readonly GameplayModel _model;
        private readonly ReputationConfig _config;
        private readonly GoodsResources _goodResources;

        private readonly List<IModifier> _modifiers = new();
        private readonly List<ReputationLogEntry> _reputationLog = new();

        public ReputationManager(Town town)
        {
            _model = GameplayContext.Instance.Model;
            _config = ConfigurationManager.Configurations.ReputationConfig;
            _goodResources = ResourceManager.Instance.GoodsResources;

            _town = town;

            Bind();
        }

        ~ReputationManager()
        {
            Unbind();
        }

        public void ApplyCaughtThief(float reputationLoss)
        {
            UpdateReputation(reputationLoss, "Your thief was caught stealing!");
        }

        public void ApplyNeglect()
        {
            var currentReputation = Reputation.Value;
            if (currentReputation <= 0)
                return;

            var activationDelay = _config.NeglectData.ActivationDelayInDays;
            var message = $"The town has been neglected for more than {activationDelay} days.";
            var clampedNeglect = Mathf.Min(_config.NeglectData.ReputationCost,
                currentReputation - _config.NeglectData.ReputationCost);
            UpdateReputation(clampedNeglect, message);
        }

        public void ApplyMissionReward(float reward)
        {
            UpdateReputation(reward, $"You supplied {_town.Name} with the goods they wanted.");
        }

        public void ApplyMissionPenalty(float penalty)
        {
            UpdateReputation(penalty, $"You failed to supply {_town.Name} in time.");
        }

        public void AddModifier(IModifier modifier)
        {
            _modifiers.Add(modifier);
        }

        public void RemoveModifier(IModifier modifier)
        {
            _modifiers.Remove(modifier);
        }

        private void Bind()
        {
            _town.TradeCompleted += OnTradeCompleted;
            _town.DevelopmentManager.Tier.Observe(OnTownUpgrade, false);
            _town.ProductionManager.ProductionAdded += OnProductionBuildingBuilt;
        }

        private void Unbind()
        {
            _town.TradeCompleted -= OnTradeCompleted;
            _town.DevelopmentManager.Tier.StopObserving(OnTownUpgrade);
            _town.ProductionManager.ProductionAdded -= OnProductionBuildingBuilt;
        }

        private void OnTradeCompleted(TradeInfo tradeInfo)
        {
            var tradeVolumePerRep = _config.RewardData.TradeVolumePerReputationPoint;
            var repChangeFloat = tradeInfo.TotalPrice / tradeVolumePerRep;
            // round to 1 digit after comma
            var finalRepChange = Mathf.Floor(repChangeFloat * 10f) * .1f * tradeInfo.HaggleLevel;
            var goodName = _goodResources.ResourceData[tradeInfo.Good].GoodName;
            var message =
                $"Traded {tradeInfo.Amount}x{goodName} worth {tradeInfo.TotalPrice} coin at haggle level {tradeInfo.HaggleLevel}";
            UpdateReputation(finalRepChange, message);
        }

        private void OnProductionBuildingBuilt(Producer producer)
        {
            var tier = producer.Tier;
            var repChange = tier switch
            {
                Tier.Tier1 => _config.RewardData.Tier1ProductionBuilding,
                Tier.Tier2 => _config.RewardData.Tier2ProductionBuilding,
                Tier.Tier3 => _config.RewardData.Tier3ProductionBuilding,
                _ => 0
            };
            var good = producer.ProducedGood;
            var message = $"Player constructed a production building ({good}) of {tier.ToDisplayString()}";
            UpdateReputation(repChange, message);
        }

        private void OnTownUpgrade(Tier tier)
        {
            var repChange = tier switch
            {
                Tier.Tier2 => _config.RewardData.TownUpgradeTier2,
                Tier.Tier3 => _config.RewardData.TownUpgradeTier3,
                _ => 0
            };

            UpdateReputation(repChange, $"{_town.Name} was upgrade to {tier.ToDisplayString()}");
        }

        private void UpdateReputation(float repChange, string reason)
        {
            // TODO - MED-73: apply modifiers
            Reputation.Value = Mathf.Clamp(Reputation.Value + repChange, -100, 100);

            var date = _model.Date;
            var logEntry = new ReputationLogEntry(date, repChange, Reputation.Value, reason);
            _reputationLog.Add(logEntry);
        }
    }
}