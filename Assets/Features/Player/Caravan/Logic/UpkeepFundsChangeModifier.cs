using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;

namespace Features.Player.Caravan.Logic
{
    public sealed class UpkeepFundsChangeModifier : FlatModifier
    {
        public UpkeepFundsChangeModifier(Observable<float> upkeep, string originName) :
            base(upkeep.Invert(), $"{originName} Upkeep") { }
    }
}