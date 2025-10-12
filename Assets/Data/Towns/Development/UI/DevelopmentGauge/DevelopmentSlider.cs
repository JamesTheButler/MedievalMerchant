using System;
using System.Collections.Generic;
using Common;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Data.Towns.Development.UI.DevelopmentGauge
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

        public void SetMilestones(List<DevelopmentMilestoneUiData> milestones)
        {
            ClearMilestones();

            var maxWidth = ((RectTransform)slider.transform).rect.width;
            foreach (var (threshold, sprite) in milestones)
            {
                var prefab = threshold.IsApproximately(100) ? endMilestonePrefab : milestonePrefab;
                var milestone = Instantiate(prefab, milestoneParent);
                var rectTransform = (milestone.transform as RectTransform)!;
                var trackPosition = maxWidth * (threshold * .01f);
                rectTransform.anchoredPosition = new Vector2(trackPosition, rectTransform.anchoredPosition.y);
                var milestoneScript = milestone.GetComponent<DevelopmentMilestone>();
                milestoneScript.SetUp(slider, sprite, threshold);
            }
        }

        public void ClearMilestones()
        {
            milestoneParent.DestroyChildren();
        }
    }
}