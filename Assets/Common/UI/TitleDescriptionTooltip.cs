using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Common.UI
{
    public sealed class TitleDescriptionTooltip : TooltipBase<(string Title, string Description)>
    {
        [SerializeField, Required]
        private TMP_Text titleTextfield;

        [SerializeField, Required]
        private TMP_Text descriptionTextfield;

        public override void Reset()
        {
            titleTextfield.text = string.Empty;
            descriptionTextfield.text = string.Empty;
        }

        protected override void UpdateUI((string Title, string Description) data)
        {
            titleTextfield.text = data.Title;
            descriptionTextfield.text = data.Description;
        }
    }
}