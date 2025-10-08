using Data.Player.Retinue.Config.CompanionDatas;
using UnityEngine;

namespace Data.Player.Retinue.Logic
{
    public sealed class DiplomatCompanionLogic : BaseCompanionLogic<DiplomatCompanionData>
    {
        protected override CompanionType Type => CompanionType.Diplomat;

        public override void SetLevel(int level)
        {
            Debug.LogError("The method or operation is not implemented.");
        }
    }
}