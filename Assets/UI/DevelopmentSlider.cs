using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DevelopmentSlider : MonoBehaviour
    {
        [SerializeField, Required]
        private Slider slider;

        // TODO - POLISH: behavior is buggy when switching between towns
        //[SerializeField]
        //private SliderAnimator sliderAnimator;

        [SerializeField, Required]
        private GameObject handle;

        [SerializeField]
        private float handleActivationThreshold;

        public void SetDevelopment(float developmentScore)
        {
            var actualValue = Math.Clamp(developmentScore, 0, 100f);
            slider.value = actualValue;
            //sliderAnimator.SetValue(actualValue, animate);
            handle.SetActive(actualValue > handleActivationThreshold);
        }
    }
}