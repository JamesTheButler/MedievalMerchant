using System;
using System.Collections.Generic;
using Common;
using Common.Modifiable;
using Common.Types;
using Features.Goods.Config;
using Features.Towns.Production.Logic;
using Features.Towns.Reputation.Config;
using Features.Trade;
using Infrastructure;
using UnityEngine;

namespace Features.Towns.Reputation.Logic
{
    public sealed class ReputationManager
    {
        private readonly Town _town;

        public Observable<float> Reputation { get; private set; } = new();

        public IReadOnlyDictionary<DateTime, ReputationLogEntry> ReputationLog => _reputationLog;
        public IReadOnlyList<IModifier> Modifiers => _modifiers;
        public Observable<bool> IsNeglected { get; private set; } = new();

        private readonly GameplayModel _model;
        private readonly ReputationConfig _config;
        private readonly GoodsResources _goodConfig;

        private readonly List<IModifier> _modifiers = new();
        private readonly Dictionary<DateTime, ReputationLogEntry> _reputationLog = new();

        private Date _neglectActivationDate = new();

        public ReputationManager(Town town)
        {
            _model = GameplayContext.Instance.Model;
            _config = ConfigurationManager.Configurations.ReputationConfig;

            _town = town;

            Bind();
            ResetNeglectDate();
        }

        public void ApplyCaughtThief(float reputationLoss)
        {
            UpdateReputation(reputationLoss, "Your thief was caught stealing!");
        }
        
        private void Bind()
        {
            _model.Date.Changed += OnDateChanged;
            _town.TradeCompleted += OnTradeCompleted;
            _town.DevelopmentManager.Tier.Observe(OnTownUpgrade, false);
            _town.ProductionManager.ProductionAdded += OnProductionBuildingBuilt;
            // TODO - Feature: Missions Completion and Failure support
            //_town.MissionManager.MissionResolved += OnMissionResolved
            //_town.MissionManager.MissionExpired += OnMissionExpired
        }

        private void Unbind()
        {
            _model.Date.Changed -= OnDateChanged;
            _town.TradeCompleted -= OnTradeCompleted;
            _town.DevelopmentManager.Tier.StopObserving(OnTownUpgrade);
            _town.ProductionManager.ProductionAdded -= OnProductionBuildingBuilt;
            // TODO - Feature: Missions Completion and Failure support
            //_town.MissionManager.MissionResolved -= OnMissionResolved
            //_town.MissionManager.MissionExpired -= OnMissionExpired
        }

        private void OnTradeCompleted(TradeInfo tradeInfo)
        {
            if (tradeInfo.HaggleLevel >= 0)
            {
                ResetNeglectDate();
            }
            
            var tradeVolumePerRep = _config.RewardData.TradeVolumePerReputationPoint;
            var repChangeFloat = tradeInfo.FinalPrice / tradeVolumePerRep;
            // round to 1 digit after comma
            var finalRepChange = Mathf.Floor(repChangeFloat * 10f) * .1f * tradeInfo.HaggleLevel;
            var goodName = _goodConfig.ConfigData[tradeInfo.Good].GoodName;
            var message =
                $"Traded {tradeInfo.Amount}x{goodName} worth {tradeInfo.FinalPrice} coin at haggle level {tradeInfo.HaggleLevel}";
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
                _ => 0,
            };
            var good = producer.ProducedGood;
            var message = $"Player constructed a production building ({good}) of {tier.ToDisplayString()}";
            UpdateReputation(repChange, message);
        }

        private void OnDateChanged(Date date)
        {
            var isNeglectDateReached = date >= _neglectActivationDate;
            var isAboveNeglectThreshold = Reputation.Value >= _config.NeglectData.ReputationThreshold;

            if (isAboveNeglectThreshold || !isNeglectDateReached)
                return;

            IsNeglected.Value = true;
            _neglectActivationDate.AddDays(_config.NeglectData.IntervalInDays);
            var message = $"The town has been neglected for more than {_config.NeglectData.ActivationDelayInDays}";
            UpdateReputation(_config.NeglectData.ReputationCost, message);
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

        private void ResetNeglectDate()
        {
            _neglectActivationDate = _model.Date + _config.NeglectData.ActivationDelayInDays;
        }

        private void UpdateReputation(float repChange, string reason)
        {
            // TODO - bug: apply modifiers
            Reputation.Value = Mathf.Clamp(Reputation.Value + repChange, -100, 100);

            if (repChange > 0)
            {
                ResetNeglectDate();
            }

            var date = _model.Date;
            var logEntry = new ReputationLogEntry(date, repChange, Reputation.Value, reason);
            _reputationLog.Add(DateTime.Now, logEntry);
        }
    }
}