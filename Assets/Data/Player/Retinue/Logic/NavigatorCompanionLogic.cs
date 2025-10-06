using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class NavigatorCompanionLogic : BaseCompanionLogic<NavigatorLevelData>
    {
        protected override CompanionType Type => CompanionType.Navigator;

        protected override void OnLevelChanged(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}