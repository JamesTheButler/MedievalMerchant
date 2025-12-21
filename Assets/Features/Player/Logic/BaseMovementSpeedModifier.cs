using Common.Infrastructure.Modifiable;

namespace Features.Player.Logic
{
    public sealed class BaseMovementSpeedModifier : BaseValueModifier
    {
        public BaseMovementSpeedModifier(float value) : base(value, "Base Movement Speed of Caravan") { }
    }
}