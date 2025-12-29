using Common.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Development.UI.DevelopmentGauge
{
    public sealed class DevelopmentMilestone : MonoBehaviour
    {
        public record Data(float ThresholdPercent, Sprite Icon, string Description);

        [SerializeField, Required]
        private Image milestoneImage, topImage, incompletedBlocker;

        [SerializeField]
        private TMP_Text percentText;

        [SerializeField, Required]
        private DevelopmentMilestoneTooltipHandler tooltip;

        private Data _data;
        private float _threshold;
        private Slider _slider;
        private bool? _isCompleted;

        public void SetUp(Slider slider, Data data)
        {
            _data = data;
            _threshold = data.ThresholdPercent * 100f;

            percentText?.SetText(data.ThresholdPercent.ToPercentString());
            _slider = slider;
            _slider.onValueChanged.AddListener(SliderValueChanged);
            milestoneImage.sprite = data.Icon;
            tooltip.SetData(new DevelopmentMilestoneTooltip.Data(data, _isCompleted ?? false));

            SliderValueChanged(_slider.value);
        }

        private void OnDestroy()
        {
            _slider?.onValueChanged.RemoveListener(SliderValueChanged);
        }

        private void SliderValueChanged(float newValue)
        {
            var isCompleted = newValue >= _threshold;
            if (_isCompleted == isCompleted)
                return;

            tooltip.SetData(new DevelopmentMilestoneTooltip.Data(_data, isCompleted));
            incompletedBlocker.gameObject.SetActive(!isCompleted);
            _isCompleted = isCompleted;
        }
    }
}