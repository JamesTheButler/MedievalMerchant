using Common.Infrastructure;
using Common.Infrastructure.Modifiable;

namespace Features.Player.Caravan.Logic
{
    public sealed class BaseUpkeepModifier : BaseValueModifier
    {
        public BaseUpkeepModifier(float value) : base(value, GetDescription()) { }

        private static string GetDescription()
        {
            return ResourceManager.Instance.LocalizationResources.Player.UpkeepBase;
        }
    }
}