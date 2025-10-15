using Common.Modifiable;

namespace Common.UI
{
    public sealed class ModifiableTooltip : TooltipBase<ModifiableVariable>
    {
        public override void SetData(ModifiableVariable data)
        {
            //data.Modifiers.Select(m => m.Description)
        }
    }
}