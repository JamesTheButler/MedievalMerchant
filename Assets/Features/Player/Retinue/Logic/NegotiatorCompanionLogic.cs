using Features.Player.Retinue.Config.CompanionDatas;

namespace Features.Player.Retinue.Logic
{
    public sealed class NegotiatorCompanionLogic : BaseCompanionLogic<NegotiatorCompanionData>
    {
        protected override CompanionType Type => CompanionType.Negotiator;

        public override void SetLevel(int level)
        {
        }
    }
}