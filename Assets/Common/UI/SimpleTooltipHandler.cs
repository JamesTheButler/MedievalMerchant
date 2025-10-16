using UnityEngine;

namespace Common.UI
{
    public sealed class SimpleTooltipHandler : TooltipHandlerBase<string>
    {
        [SerializeField]
        private string defaultText;

        protected override void Start()
        {
            base.Start();
            if (!string.IsNullOrEmpty(defaultText))
            {
                SetData(defaultText);
            }
        }
    }
}