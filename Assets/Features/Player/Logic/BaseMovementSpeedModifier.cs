using Common.Infrastructure.Modifiable;

namespace Features.Player
{
    public sealed class BaseMovementSpeedModifier : BaseValueModifier
    {
        public BaseMovementSpeedModifier(float value) : base(value, "Base Movement Speed of Caravan") { }
    }
}