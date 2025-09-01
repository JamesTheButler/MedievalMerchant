using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DevelopmentSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        // TODO: behavior is buggy when switching between towns
        //[SerializeField]
        //private SliderAnimator sliderAnimator;

        [SerializeField]
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