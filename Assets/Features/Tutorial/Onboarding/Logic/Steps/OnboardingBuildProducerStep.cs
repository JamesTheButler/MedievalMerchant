using System.Collections;
using Common.Types;
using Features.Towns;
using Features.Towns.Production.Logic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

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
            while (!_tradeBuildingExists)
            {
                yield return new WaitForEndOfFrame();
                controller.Blink(_town, MouseButton.Left);
                yield return new WaitUntil(() => controller.TownUI.IsOpen);

                var cell = controller.TownProducerUI.GetCell(1, Tier.Tier1);

                if (!cell)
                {
                    Debug.LogError($"Could not find cell for good '{_good}'.");
                    continue;
                }

                controller.Blink(cell, MouseButton.Left);

                if (_tradeBuildingExists)
                    break;

                yield return new WaitUntil(() => _tradeBuildingExists);
            }

            controller.HideBlinker();

            yield return null;
        }

        public void CleanUp()
        {
            _town.ProductionManager.ProductionAdded.StopObserving(OnProducerAdded);
        }
    }
}