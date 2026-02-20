using UnityEngine;
using UnityEngine.Localization;

namespace Common.UI.Tooltips
{
    public sealed class TitleDescriptionTooltipHandler : TooltipHandlerBase<(string Title, string Description)>
    {
        [SerializeField]
        private LocalizedString defaultTitleString, defaultDescriptionString;

        [SerializeField]
        private string defaultTitle, defaultDescription;

        protected override void Start()
        {
            base.Start();

            var finalTitle = defaultTitleString.IsEmpty ? defaultTitle : defaultTitleString.GetLocalizedString();
            var finalDescription = defaultDescriptionString.IsEmpty
                ? defaultDescription
                : defaultDescriptionString.GetLocalizedString();

            SetData((finalTitle, finalDescription));
        }
    }
}