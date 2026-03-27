using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Common.Utility;
using Features.Towns.Reputation.Data;
using UnityEngine;

namespace Features.Towns.Reputation.Logic
{
    public sealed class ReputationModel
    {
        public IReadOnlyObservable<float> Reputation => _reputation;
        public IReadOnlyList<ReputationLogEntry> ReputationLog => _reputationLog;
        public IReadOnlyList<BasePercentageModifier> Modifiers => _modifiers;

        public Observable<bool> IsNeglected { get; } = new();

        private readonly Observable<float> _reputation = new();
        private readonly List<ReputationLogEntry> _reputationLog = new();
        private readonly ObservableSum _modifierSum = new();
        private readonly List<BasePercentageModifier> _modifiers = new();

        public void UpdateReputation(float repChange, string reason)
        {
            if (repChange.IsApproximately(0))
                return;

            var modifiedRepChange = repChange * (1 + _modifierSum);
            _reputation.Value = Mathf.Clamp(Reputation.Value + modifiedRepChange, -100, 100);

            var date = GameplayContext.Instance.Model.DateModel;
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
    }
}