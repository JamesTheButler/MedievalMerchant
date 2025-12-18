using UnityEngine;

namespace Common.UI.Tooltips
{
    public sealed class TitleDescriptionTooltipHandler : TooltipHandlerBase<(string Title, string Description)>
    {
        [SerializeField]
        private string defaultTitle, defaultDescription;

        protected override void Start()
        {
            base.Start();
            if (!string.IsNullOrEmpty(defaultTitle))
            {
                SetData((defaultTitle, defaultDescription));
            }
        }
    }
}