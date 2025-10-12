using Features.Player.Retinue.Config.CompanionDatas;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class ArchitectCompanionLogic : BaseCompanionLogic<ArchitectCompanionData>
    {
        protected override CompanionType Type => CompanionType.Architect;

        public override void SetLevel(int level)
        {
            Debug.LogError("The method or operation is not implemented.");
        }
    }
}