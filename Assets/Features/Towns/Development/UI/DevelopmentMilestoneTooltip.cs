using Common;
using Common.UI;
using Features.Towns.Development.Config;
using Features.Towns.Development.UI.DevelopmentGauge;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.Development.UI
{
    public class DevelopmentMilestoneTooltip : TooltipBase<DevelopmentMilestoneTooltip.Data>
    {
        public record Data(DevelopmentMilestone.Data MilestoneData, bool IsUnlocked);

        [SerializeField, Required]
        private TMP_Text descriptionText, effectsText;

        [SerializeField, Required]
        private Image milestoneIcon;

        public override void SetData(Data data)
        {
            milestoneIcon.sprite = data.MilestoneData.Icon;
            descriptionText.text = $"Unlocks at development {data.MilestoneData.ThresholdPercent.ToPercentString()}";
            effectsText.text = data.MilestoneData.Description;
            descriptionText.gameObject.SetActive(!data.IsUnlocked);
        }

        public override void Reset() { }
    }
}