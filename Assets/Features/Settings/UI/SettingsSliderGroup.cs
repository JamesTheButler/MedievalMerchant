using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Settings.UI
{
    public sealed class SettingsSliderGroup : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text valueText;

        [SerializeField, Required]
        private Slider slider;

        private void Awake()
        {
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            valueText.text = $"{(int)value}";
        }
    }
}