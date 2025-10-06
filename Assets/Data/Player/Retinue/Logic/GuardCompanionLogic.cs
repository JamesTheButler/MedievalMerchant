using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class GuardCompanionLogic : BaseCompanionLogic<GuardLevelData>
    {
        protected override CompanionType Type => CompanionType.Guard;

        protected override void OnLevelChanged(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}