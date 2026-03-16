using System;
using Common.Utility;
using UnityEngine.Localization.SmartFormat.Core.Extensions;

namespace Editor
{
    [Serializable]
    public class FallbackArgumentSource : ISource
    {
        public bool TryEvaluateSelector(ISelectorInfo selectorInfo)
        {
            if (selectorInfo.CurrentValue != null)
                return false;
            var trimmedName = selectorInfo.SelectorText.TrimStart("_int_");
            selectorInfo.Result = $"[{trimmedName}]";
            return true;
        }
    }
}