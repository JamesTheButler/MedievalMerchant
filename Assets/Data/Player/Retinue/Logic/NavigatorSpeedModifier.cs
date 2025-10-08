using Data.Modifiable;

namespace Data.Player.Retinue.Logic
{
    public sealed class NavigatorSpeedModifier : BasePercentageModifier
    {
        public NavigatorSpeedModifier(float value, int level) : base(value, $"Navigator level {level}")
        {
        }
    }
}