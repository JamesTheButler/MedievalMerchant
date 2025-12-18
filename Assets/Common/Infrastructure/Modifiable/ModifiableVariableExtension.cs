using System.Collections.Generic;

namespace Common.Infrastructure.Modifiable
{
    public static class ModifiableVariableExtension
    {
        public static void AddModifiers(this ModifiableVariable variable, IEnumerable<IModifier> modifiers)
        {
            foreach (var modifier in modifiers)
            {
                variable.AddModifier(modifier);
            }
        }
    }
}