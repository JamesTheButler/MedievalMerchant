using Data.Player.Retinue.Config;

namespace Data.Player.Retinue.Logic
{
    public sealed class NegotiatorCompanionLogic : BaseCompanionLogic<NegotiatorLevelData>
    {
        protected override CompanionType Type => CompanionType.Negotiator;

        protected override void OnLevelChanged(int level)
        {
            throw new System.NotImplementedException();
        }
    }
}