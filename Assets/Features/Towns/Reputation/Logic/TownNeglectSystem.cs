using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Towns.Reputation.Data;

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

            _town.ReputationManager.Reputation.Observe(OnReputationChanged);
            _model.Date.Changed.Observe(OnGameDateChanged);

            ResetNeglect();
        }

        public void CleanUp()
        {
            _town.ReputationManager.Reputation.StopObserving(OnReputationChanged);
            _model.Date.Changed.StopObserving(OnGameDateChanged);
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

            _town.ReputationManager.IsNeglected.Value = true;
            _town.ReputationManager.ApplyNeglect();
            _neglectActivationDate = _model.Date + _neglectConfig.IntervalInDays;
        }

        private void ResetNeglect()
        {
            _neglectActivationDate = _model.Date + _neglectConfig.ActivationDelayInDays;
            _town.ReputationManager.IsNeglected.Value = false;
        }
    }
}