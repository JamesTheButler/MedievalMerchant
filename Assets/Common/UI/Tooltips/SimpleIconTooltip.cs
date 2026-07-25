using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Tooltips
{
    public sealed class SimpleIconTooltip : TooltipBase<SimpleIconTooltip.Data>
    {
        public sealed record Data(Sprite Icon, string Text);

        [SerializeField, Required]
        private Image image;

        [SerializeField, Required]
        private TMP_Text textfield;

        protected override void UpdateUI(Data data)
        {
            image.sprite = data.Icon;
            textfield.text = data.Text;
        }

        public override void Reset()
        {
            textfield.text = string.Empty;
            image.sprite = null;
        }
    }
}