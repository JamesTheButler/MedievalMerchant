using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class DiplomatCompanionLogic : BaseCompanionLogic<DiplomatLevelData>
    {
        protected override CompanionType Type => CompanionType.Diplomat;

        protected override void OnLevelChanged(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}