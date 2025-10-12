using System;
using System.Collections.Generic;
using Common;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Development.UI.DevelopmentGauge
{
    public class DevelopmentSlider : MonoBehaviour
    {
        [SerializeField, Required]
        private Slider slider;

        [SerializeField, Required]
        private GameObject milestonePrefab;

        [SerializeField, Required]
        private GameObject endMilestonePrefab;

        [SerializeField, Required]
        private Transform milestoneParent;

        public void SetDevelopment(float developmentScore)
        {
            var actualValue = Math.Clamp(developmentScore, 0, 100f);
            slider.value = actualValue;
        }

        public void SetMilestones(List<DevelopmentMilestone.Data> milestones)
        {
            ClearMilestones();

            var maxWidth = ((RectTransform)slider.transform).rect.width;
            foreach (var milestoneData in milestones)
            {
                var threshold = milestoneData.Threshold;
                var prefab = threshold.IsApproximately(100) ? endMilestonePrefab : milestonePrefab;
                var milestone = Instantiate(prefab, milestoneParent);

                // set up logic
                var milestoneScript = milestone.GetComponent<DevelopmentMilestone>();
                milestoneScript.SetUp(slider, milestoneData);

                // set up positioning
                var rectTransform = (milestone.transform as RectTransform)!;
                var trackPosition = maxWidth * (threshold * .01f);
                rectTransform.anchoredPosition = new Vector2(trackPosition, rectTransform.anchoredPosition.y);
            }
        }

        public void ClearMilestones()
        {
            milestoneParent.DestroyChildren();
        }
    }
}