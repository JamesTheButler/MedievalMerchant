using Data.Player.Retinue.Config;
using UnityEngine;

namespace Data.Player.Retinue.Logic
{
    public sealed class NegotiatorCompanionLogic : BaseCompanionLogic<NegotiatorCompanionData>
    {
        protected override CompanionType Type => CompanionType.Negotiator;

        public override void SetLevel(int level)
        {
            Debug.LogError("The method or operation is not implemented.");
        }
    }
}