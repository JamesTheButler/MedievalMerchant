using Common.Modifiable;

namespace Features.Player.Retinue.Logic
{
    public sealed class NavigatorSpeedModifier : BasePercentageModifier
    {
        public NavigatorSpeedModifier(float value, int level) : base(value, $"Navigator level {level}")
        {
        }
    }
}