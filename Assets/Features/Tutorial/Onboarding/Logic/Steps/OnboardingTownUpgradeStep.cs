using System.Collections;
using Common.Types;
using Features.Towns;
using UnityEngine;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    public sealed class OnboardingTownUpgradeStep : IOnboardingStep
    {
        private readonly Town _town;
        private readonly Tier _tier;
        private readonly int _messageIndex;

        public OnboardingTask Task { get; }

        private bool _isCompleted;

        public OnboardingTownUpgradeStep(Town town, Tier tier, OnboardingTask task)
        {
            _town = town;
            _tier = tier;
            Task = task;
        }

        public void Initialize()
        {
            _town.Tier.Observe(OnTierChanged);
        }

        private void OnTierChanged(Tier tier)
        {
            if (tier >= _tier)
            {
                _isCompleted = true;
            }
        }

        public IEnumerator Run(OnboardingController controller)
        {
            yield return new WaitUntil(() => _isCompleted);
        }

        public void CleanUp() { }
    }
}