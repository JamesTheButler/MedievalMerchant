using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class ThiefCompanionLogic : BaseCompanionLogic<GuardCompanionData>
    {
        protected override CompanionType Type => CompanionType.Guard;

        public override void SetLevel(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}