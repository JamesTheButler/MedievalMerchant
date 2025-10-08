using Data.Player.Retinue.Config;
using UnityEngine;

namespace Data.Player.Retinue.Logic
{
    public sealed class GuardCompanionLogic : BaseCompanionLogic<GuardCompanionData>
    {
        protected override CompanionType Type => CompanionType.Guard;

        public override void SetLevel(int level)
        {
            Debug.LogError("The method or operation is not implemented.");
        }
    }
}