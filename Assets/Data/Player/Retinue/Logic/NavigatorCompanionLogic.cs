using Data.Player.Retinue.Config.CompanionDatas;

namespace Data.Player.Retinue.Logic
{
    public sealed class NavigatorCompanionLogic : BaseCompanionLogic<NavigatorCompanionData>
    {
        protected override CompanionType Type => CompanionType.Navigator;

        private NavigatorSpeedModifier _activeSpeedModifier;

        public override void SetLevel(int level)
        {
            var player = Model.Instance.Player;

            var newModifier = new NavigatorSpeedModifier(ConfigData.GetTypedLevelData(level).SpeedBonus, level);

            if (_activeSpeedModifier != null)
            {
                player.MovementSpeed.RemoveModifier(_activeSpeedModifier);
            }

            player.MovementSpeed.AddModifier(newModifier);
            _activeSpeedModifier = newModifier;
        }
    }
}