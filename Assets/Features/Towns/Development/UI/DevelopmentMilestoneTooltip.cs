using Common.UI.Tooltips;
using Common.Utility;
using Features.Localization.UI;
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
        private TMP_Text effectsText;

        [SerializeField, Required]
        private LocalizedText descriptionText;

        [SerializeField, Required]
        private Image milestoneIcon;

        protected override void UpdateUI(Data data)
        {
            milestoneIcon.sprite = data.MilestoneData.Icon;

            var args = new { Percentage = data.MilestoneData.ThresholdPercent.ToPercentString() };
            descriptionText.SetArgs(args);
            effectsText.text = data.MilestoneData.Description;
            descriptionText.gameObject.SetActive(!data.IsUnlocked);
        }

        public override void Reset() { }
    }
}