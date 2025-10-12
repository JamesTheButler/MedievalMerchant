using System;
using Data.Configuration;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Towns.Development.UI.DevelopmentGauge
{
    public sealed class DevelopmentMilestone : MonoBehaviour
    {
        [SerializeField, Required]
        private Image milestoneImage;

        [SerializeField]
        private Image baseImage;

        [SerializeField, Required]
        private Image topImage;

        private readonly Lazy<DevelopmentMilestoneAssets> _milestoneAssets =
            new(() => ConfigurationManager.Instance.DevelopmentMilestoneAssets);

        private float _thresholdPercent;
        private Slider _slider;
        private bool? _isCompleted;


        public void SetUp(Slider newSlider, Sprite icon, float threshold)
        {
            _thresholdPercent = threshold;

            _slider = newSlider;
            _slider.onValueChanged.AddListener(SliderValueChanged);
            milestoneImage.sprite = icon;

            SliderValueChanged(_slider.value);
        }

        private void OnDestroy()
        {
            _slider?.onValueChanged.RemoveListener(SliderValueChanged);
        }

        private void SliderValueChanged(float newValue)
        {
            var isCompleted = newValue >= _thresholdPercent;
            if (_isCompleted == isCompleted)
                return;

            var assets = _milestoneAssets.Value;

            // to accomodate base-less end milestones
            if (baseImage)
            {
                baseImage.sprite =
                    isCompleted ? assets.BaseComplete : assets.BaseIncomplete;
            }

            topImage.sprite = isCompleted ? assets.TopComplete : assets.TopIncomplete;

            _isCompleted = isCompleted;
        }
    }
}