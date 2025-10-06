using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class ArchitectCompanionLogic : BaseCompanionLogic<ArchitectLevelData>
    {
        protected override CompanionType Type => CompanionType.Architect;

        protected override void OnLevelChanged(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}