using System.Collections;
using Common.Infrastructure.Gameplay;
using Features.Player.Logic;

namespace Features.Tutorial.Onboarding.Logic.Steps
{
    /// <summary>
    /// Step tp ensure that the player has enough money to complete the next step in the tutorial.
    /// </summary>
    public sealed class OnboardingEnsureFundsStep : IOnboardingStep
    {
        public OnboardingTask Task => null;

        private readonly int _targetFunds;
        private PlayerModel _player;

        public OnboardingEnsureFundsStep(int targetFunds)
        {
            _targetFunds = targetFunds;
        }

        public void Initialize()
        {
            _player = GameplayContext.Instance.Model.Player;
        }

        public void CleanUp() { }

        public IEnumerator Run(OnboardingController controller)
        {
            if (_player.Inventory.HasFunds(_targetFunds))
                yield return null;

            _player.Inventory.AddFunds(_targetFunds - _player.Inventory.Funds.Value);
            yield return null;
        }
    }
}