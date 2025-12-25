using Features.Player.Retinue.Config.CompanionDatas;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class GuardCompanionLogic : BaseCompanionLogic<GuardCompanionData>
    {
        protected override CompanionType Type => CompanionType.Guard;

        public override void SetLevel(int level)
        {
            Debug.LogWarning("Guard is not yet implemented.");
        }
    }
}