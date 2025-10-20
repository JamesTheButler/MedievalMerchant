using System;
using Common;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Development.UI.DevelopmentGauge
{
    public sealed class DevelopmentMilestone : MonoBehaviour
    {
        public record Data(float ThresholdPercent, Sprite Icon, string Description);

        [SerializeField, Required]
        private Image milestoneImage, topImage;

        [SerializeField]
        private Image baseImage;

        [SerializeField, Required]
        private DevelopmentMilestoneTooltipHandler tooltip;

        private readonly Lazy<DevelopmentMilestoneAssets> _milestoneAssets =
            new(() => ConfigurationManager.Instance.DevelopmentMilestoneAssets);

        private Data _data;
        private float _threshold;
        private Slider _slider;
        private bool? _isCompleted;


        public void SetUp(Slider newSlider, Data data)
        {
            _data = data;
            _threshold = data.ThresholdPercent * 100f;

            _slider = newSlider;
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
            var assets = _milestoneAssets.Value;

            // to accomodate base-less end milestones
            if (baseImage)
            {
                baseImage.sprite = isCompleted ? assets.BaseComplete : assets.BaseIncomplete;
            }

            topImage.sprite = isCompleted ? assets.TopComplete : assets.TopIncomplete;

            _isCompleted = isCompleted;
        }
    }
}