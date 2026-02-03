using System.Collections;
using Common.Infrastructure.Gameplay;
using Features.Player.Logic;
using Features.Towns;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingTravelStep : IOnboardingStep
    {
        public OnboardingTask Task { get; }

        private readonly Town _town;
        private PlayerLocation _playerLocation;

        private bool _hasArrived;

        public OnboardingTravelStep(Town town, OnboardingTask task = null)
        {
            _town = town;
            Task = task;
        }

        public void Initialize()
        {
            _playerLocation = GameplayContext.Instance.Model.Player.Location;
            _playerLocation.CurrentTown.Observe(OnTownChanged);
        }

        private void OnTownChanged(Town town)
        {
            if (town == _town)
            {
                _hasArrived = true;
            }
        }

        public IEnumerator Run(OnboardingController controller)
        {
            controller.Blink(_town, MouseButton.Right);
            yield return new WaitUntil(() => _hasArrived);
            controller.HideBlinker();
        }

        public void CleanUp()
        {
            _playerLocation.CurrentTown.StopObserving(OnTownChanged);
        }
    }
}