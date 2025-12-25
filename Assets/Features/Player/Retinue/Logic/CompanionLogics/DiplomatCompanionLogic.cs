using Features.Player.Retinue.Config.CompanionDatas;
using UnityEngine;

namespace Features.Player.Retinue.Logic.CompanionLogics
{
    public sealed class DiplomatCompanionLogic : BaseCompanionLogic<DiplomatCompanionData>
    {
        protected override CompanionType Type => CompanionType.Diplomat;

        public override void SetLevel(int level)
        {
            Debug.LogWarning("Diplomat is not yet implemented.");
        }
    }
}