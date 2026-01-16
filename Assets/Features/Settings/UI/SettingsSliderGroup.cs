using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Settings.UI
{
    public sealed class SettingsSliderGroup : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text valueText;

        public void UpdateText(float value)
        {
            valueText.text = $"{(int)value}";
        }
    }
}