using Data.Player.Retinue.Config.CompanionDatas;

namespace Data.Player.Retinue.Logic
{
    public sealed class NavigatorCompanionLogic : BaseCompanionLogic<NavigatorCompanionData>
    {
        protected override CompanionType Type => CompanionType.Navigator;

        public override void SetLevel(int level)
        {
        }
    }
}