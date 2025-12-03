using Common;
using Common.Modifiable;

namespace Features.Player.Caravan.Logic
{
    public sealed class UpkeepFundsChangeModifier : FlatModifier
    {
        public UpkeepFundsChangeModifier(Observable<float> upkeep) :
            base(
                upkeep.Transform(value => -value),
                "Caravan and Retinue Upkeep") { }
    }
}