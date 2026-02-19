using UnityEngine;
using UnityEngine.Localization;

namespace Common.UI.Tooltips
{
    public sealed class SimpleTooltipHandler : TooltipHandlerBase<string>
    {
        [SerializeField]
        private LocalizedString text;
        
        [SerializeField]
        private string defaultText;

        protected override void Start()
        {
            base.Start();

            if (!text.IsEmpty)
            {
                SetData(text.GetLocalizedString());
                return;
            }
            
            if (!string.IsNullOrEmpty(defaultText))
            {
                SetData("<not localized>"+defaultText);
            }
        }
    }
}