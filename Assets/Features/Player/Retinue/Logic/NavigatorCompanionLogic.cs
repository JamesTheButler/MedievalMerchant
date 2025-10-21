using Common;
using Features.Player.Retinue.Config.CompanionDatas;

namespace Features.Player.Retinue.Logic
{
    public sealed class NavigatorCompanionLogic : BaseCompanionLogic<NavigatorCompanionData>
    {
        protected override CompanionType Type => CompanionType.Navigator;

        private PlayerModel _player;

        private NavigatorSpeedModifier _activeSpeedModifier;
        private NavigatorUpkeepModifier _activeUpkeepModifier;

        public override void SetLevel(int level)
        {
            _player = Model.Instance.Player;

            HandleSpeedModifier(level);
            HandleUpkeepModifier(level);
        }

        private void HandleSpeedModifier(int level)
        {
            if (_activeSpeedModifier == null)
            {
                _activeSpeedModifier = new NavigatorSpeedModifier(level);
                _player.MovementSpeed.AddModifier(_activeSpeedModifier);
                return;
            }

            _activeSpeedModifier.Update(level);
        }

        private void HandleUpkeepModifier(int level)
        {
            if (_activeUpkeepModifier == null)
            {
                _activeUpkeepModifier = new NavigatorUpkeepModifier(level);
                _player.CaravanManager.Upkeep.AddModifier(_activeUpkeepModifier);
                return;
            }

            _activeUpkeepModifier.Update(level);
        }
    }
}