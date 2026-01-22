using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Types;
using Common.Utility;
using Features.Towns.Production.Logic;
using Features.Towns.Reputation.Data;
using UnityEngine;

namespace Features.Towns.Reputation.Logic
{
    public sealed class ReputationManager
    {
        public IReadOnlyObservable<float> Reputation => _reputation;
        public IReadOnlyList<ReputationLogEntry> ReputationLog => _reputationLog;
        public Observable<bool> IsNeglected { get; } = new();
        public IReadOnlyList<BasePercentageModifier> Modifiers  =>_modifiers;

        private readonly Town _town;
        private readonly GameplayModel _model;
        private readonly ReputationConfig _reputationConfig;
        private readonly Observable<float> _reputation = new();
        private readonly List<ReputationLogEntry> _reputationLog = new();
        private readonly ObservableSum _modifierSum = new();
        private readonly List<BasePercentageModifier> _modifiers = new();
        

        public ReputationManager(Town town)
        {
            _model = GameplayContext.Instance.Model;
            _reputationConfig = ConfigurationManager.Configurations.ReputationConfig;

            _town = town;

            Bind();
        }

        ~ReputationManager()
        {
            Unbind();
        }

        public void UpdateReputation(float repChange, string reason)
        {
            if (repChange.IsApproximately(0))
                return;

            var modifiedRepChange = repChange * (1 + _modifierSum);
            _reputation.Value = Mathf.Clamp(Reputation.Value + modifiedRepChange, -100, 100);

            var date = _model.DateModel;
            var logEntry = new ReputationLogEntry(date, modifiedRepChange, Reputation.Value, reason);
            _reputationLog.Add(logEntry);
        }

        public void AddModifier(BasePercentageModifier modifier)
        {
            _modifierSum.AddValue(modifier.Value);
            _modifiers.Add(modifier);
        }

        public void RemoveModifier(BasePercentageModifier modifier)
        {
            _modifierSum.RemoveValue(modifier.Value);
            _modifiers.Remove(modifier);
        }

        public void ApplyNeglect()
        {
            var currentReputation = Reputation.Value;
            if (currentReputation <= 0)
                return;

            var activationDelay = _reputationConfig.NeglectData.ActivationDelayInDays;
            var message = $"The town has been neglected for more than {activationDelay} days.";
            var clampedNeglect = Mathf.Min(_reputationConfig.NeglectData.ReputationCost,
                currentReputation - _reputationConfig.NeglectData.ReputationCost);
            UpdateReputation(clampedNeglect, message);
        }

        private void Bind()
        {
            _town.ProductionManager.ProductionAdded += OnProductionBuildingBuilt;
        }

        private void Unbind()
        {
            _town.ProductionManager.ProductionAdded -= OnProductionBuildingBuilt;
        }

        private void OnProductionBuildingBuilt(Producer producer)
        {
            var tier = producer.Tier;
            var repChange = tier switch
            {
                Tier.Tier1 => _reputationConfig.RewardData.Tier1ProductionBuilding,
                Tier.Tier2 => _reputationConfig.RewardData.Tier2ProductionBuilding,
                Tier.Tier3 => _reputationConfig.RewardData.Tier3ProductionBuilding,
                _ => 0
            };
            var good = producer.ProducedGood;
            var message = $"Player constructed a production building ({good}) of {tier.ToDisplayString()}";
            UpdateReputation(repChange, message);
        }
    }
}