using System.Collections;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Towns;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    /// <summary>
    /// Step tp ensure that the given town has enough of the given good to complete the next step in the tutorial.
    /// </summary>
    public sealed class OnboardingEnsureInventoryStep : IOnboardingStep
    {
        public OnboardingTask Task => null;

        private readonly Good _targetGood;
        private readonly int _targetGoodCount;
        private readonly Town _town;

        public OnboardingEnsureInventoryStep(Town town, Good targetGood, int targetGoodCount)
        {
            _town = town;
            _targetGood = targetGood;
            _targetGoodCount = targetGoodCount;
        }

        public void Initialize() { }
        public void CleanUp() { }

        public IEnumerator Run(OnboardingController controller)
        {
            if (_town.Inventory.HasGood(_targetGood, _targetGoodCount))
                yield return null;

            var currentCount = _town.Inventory.Get(_targetGood);
            _town.Inventory.AddGood(_targetGood, _targetGoodCount - currentCount);
            yield return null;
        }
    }
}