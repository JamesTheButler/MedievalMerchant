using Data.Modifiable;

namespace Data.Player
{
    public sealed class BaseMovementSpeedModifier : BaseValueModifier
    {
        public BaseMovementSpeedModifier(float value) : base(value, "Base Movement Speed of Caravan")
        {
        }
    }
}