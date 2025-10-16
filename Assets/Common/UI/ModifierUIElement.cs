using Common.Modifiable;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Common.UI
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
            var coloredText = isGood
                ? TMP.ColorGood(modifier.FormattedValue)
                : TMP.ColorBad(modifier.FormattedValue);
            valueText.text = coloredText;
        }
    }
}