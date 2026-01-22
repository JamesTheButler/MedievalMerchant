using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Towns.Reputation.Data;
using UnityEngine;

namespace Features.Towns.Reputation.Logic
{
    public sealed class TownNeglectSystem : ISystem
    {
        private readonly Town _town;

        private GameplayModel _model;
        private NeglectData _neglectConfig;

        private Date _neglectActivationDate;

        public TownNeglectSystem(Town town)
        {
            _town = town;
        }

        public void Initialize()
        {
            _model = GameplayContext.Instance.Model;
            _neglectConfig = ConfigurationManager.Configurations.ReputationConfig.NeglectData;

            _town.ReputationModel.Reputation.Observe(OnReputationChanged);
            _model.DateModel.GameDate.Observe(OnGameDateChanged);

            ResetNeglect();
        }

        public void CleanUp()
        {
            _town.ReputationModel.Reputation.StopObserving(OnReputationChanged);
            _model.DateModel.GameDate.StopObserving(OnGameDateChanged);
        }

        private void OnReputationChanged(float oldRep, float newRep)
        {
            if (oldRep > newRep || newRep <= 0)
            {
                ResetNeglect();
            }
        }

        private void OnGameDateChanged(Date date)
        {
            if (date < _neglectActivationDate)
                return;

            var currentRep = _town.ReputationModel.Reputation.Value;
            if (currentRep <= 0)
                return;

            var activationDelay = _neglectConfig.ActivationDelayInDays;
            var message = $"The town has been neglected for more than {activationDelay} days.";
            var clampedNeglect = Mathf.Min(_neglectConfig.ReputationCost, currentRep - _neglectConfig.ReputationCost);
            _town.ReputationModel.IsNeglected.Value = true;
            _town.ReputationModel.UpdateReputation(clampedNeglect, message);

            _neglectActivationDate = _model.DateModel.GameDate.Value + _neglectConfig.IntervalInDays;
        }

        private void ResetNeglect()
        {
            _neglectActivationDate = _model.DateModel.GameDate.Value + _neglectConfig.ActivationDelayInDays;
            _town.ReputationModel.IsNeglected.Value = false;
        }
    }
}