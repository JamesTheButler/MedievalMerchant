using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;

namespace Features.Player.Caravan.Logic
{
    public sealed class UpkeepFundsChangeModifier : FlatModifier
    {
        public UpkeepFundsChangeModifier(Observable<float> upkeep) :
            base(upkeep.Invert(), "Caravan Upkeep") { }
    }
}