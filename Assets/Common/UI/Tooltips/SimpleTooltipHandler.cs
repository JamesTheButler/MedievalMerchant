using UnityEngine;
using UnityEngine.Localization;

namespace Common.UI.Tooltips
{
    public sealed class SimpleTooltipHandler : TooltipHandlerBase<string>
    {
        [SerializeField]
        private LocalizedString text;

        protected override void Start()
        {
            base.Start();

            if (text.IsEmpty)
                return;

            SetData(text.GetLocalizedString());
        }
    }
}