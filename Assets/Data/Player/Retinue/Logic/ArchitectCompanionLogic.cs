using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class ArchitectCompanionLogic : BaseCompanionLogic<ArchitectCompanionData>
    {
        protected override CompanionType Type => CompanionType.Architect;

        public override void SetLevel(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}