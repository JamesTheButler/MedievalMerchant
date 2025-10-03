using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class DiplomatCompanionLogic : BaseCompanionLogic<DiplomatCompanionData>
    {
        protected override CompanionType Type => CompanionType.Diplomat;

        public override void SetLevel(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}