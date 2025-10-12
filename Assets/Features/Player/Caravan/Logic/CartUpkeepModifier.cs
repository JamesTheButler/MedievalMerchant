using Common.Modifiable;

namespace Features.Player.Caravan.Logic
{
    public sealed class CartUpkeepModifier : FlatModifier
    {
        public CartUpkeepModifier(float value, int cartLevel) : base(value, $"Upkeep for Cart level {cartLevel}")
        {
        }
    }
}