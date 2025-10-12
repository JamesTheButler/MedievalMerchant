using Common.Modifiable;

namespace Features.Player.Caravan.Logic
{
    public sealed class BaseUpkeepModifier : BaseValueModifier
    {
        public BaseUpkeepModifier(float value) : base(value, "Base Upkeep")
        {
        }
    }
}