using Data.Modifiable;

namespace Data.Player.Caravan.Logic
{
    public sealed class BaseUpkeepModifier : BaseValueModifier
    {
        public BaseUpkeepModifier(float value) : base(value, "Base Upkeep")
        {
        }
    }
}