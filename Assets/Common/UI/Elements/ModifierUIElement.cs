using Common.Infrastructure.Modifiable;
using Common.UI.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Common.UI.Elements
{
    public sealed class ModifierUIElement : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text valueText, descriptionText;

        public void SetUp(IModifier modifier, bool useDynamicColor, bool isBiggerBetter)
        {
            descriptionText.text = modifier.Description;

            if (!useDynamicColor)
            {
                valueText.text = modifier.FormattedValue;
                return;
            }

            var isGood = isBiggerBetter ? modifier.Value > 0 : modifier.Value < 0;

            var modifierValue = modifier.FormattedValue.Value;
            var style = isGood ? Style.Good : Style.Bad;
            var coloredText = modifierValue.WithStyle(style);
            valueText.text = coloredText;
        }
    }
}