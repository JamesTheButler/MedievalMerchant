using Common.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace Common.UI.Tooltips
{
    public sealed class TitleDescriptionTooltipHandler : TooltipHandlerBase<(string Title, string Description)>
    {
        [SerializeField]
        private LocalizedString defaultTitleString, defaultDescriptionString;

        protected override void Start()
        {
            base.Start();

            var finalTitle = defaultTitleString.GetLocalizedStringOptional();
            var finalDescription = defaultDescriptionString.GetLocalizedStringOptional();

            SetData((finalTitle, finalDescription));
        }
    }
}