using System.Collections.Generic;

namespace Common.Modifiable
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