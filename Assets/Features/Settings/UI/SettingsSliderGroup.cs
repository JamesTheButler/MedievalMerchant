using Common.UI.Elements;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Settings.UI
{
    public sealed class SettingsSliderGroup : InitializableBehavior
    {
        [SerializeField, Required]
        private TMP_Text valueText;

        [SerializeField, Required]
        private Slider slider;

        public override void Initialize()
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            valueText.text = $"{(int)value}";
        }
    }
}