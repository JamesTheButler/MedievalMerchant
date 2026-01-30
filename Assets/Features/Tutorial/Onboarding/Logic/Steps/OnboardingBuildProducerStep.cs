using System.Collections;
using Common.Types;
using Features.Towns;
using Features.Towns.Production.Logic;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingBuildProducerStep : IOnboardingStep
    {
        public OnboardingTask Task { get; }

        private readonly Town _town;
        private readonly Good _good;

        private bool _tradeBuildingExists;

        public OnboardingBuildProducerStep(Town town, Good good, OnboardingTask task)
        {
            _town = town;
            _good = good;
            Task = task;
        }

        public void Initialize()
        {
            if (_town.ProductionManager.IsProduced(_good))
            {
                _tradeBuildingExists = true;
                return;
            }

            _town.ProductionManager.ProductionAdded.Observe(OnProducerAdded);
        }

        private void OnProducerAdded(Producer producer)
        {
            if (producer.ProducedGood != _good)
                return;

            _tradeBuildingExists = true;
        }

        public IEnumerator Run(OnboardingController controller)
        {
            yield return new WaitUntil(() => _tradeBuildingExists);
        }

        public void CleanUp()
        {
            _town.ProductionManager.ProductionAdded.StopObserving(OnProducerAdded);
        }
    }
}