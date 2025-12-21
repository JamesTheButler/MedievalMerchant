using Common.Infrastructure;
using Common.UI.Tooltips;
using Features.Player.Retinue.Config;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class CompanionTooltip : TooltipBase<CompanionTooltip.Data>
    {
        public sealed record Data(CompanionType CompanionType, int Level);
        
        [SerializeField, Required]
        private GameObject upkeepLine;

        [SerializeField, Required]
        private TMP_Text titleText, descriptionText, effectsText, upkeepText;

        private CompanionConfig _configData;

        protected override void Awake()
        {
            base.Awake();

            _configData = ConfigurationManager.Configurations.CompanionConfig;
        }

        protected override void UpdateUI(Data data)
        {
            var companionData = _configData.Get(data.CompanionType);
            var levelData = companionData.GetLevelData(data.Level);

            titleText.text = companionData.DisplayString(data.Level);
            descriptionText.text = companionData.Description;

            upkeepLine.SetActive(levelData != null);
            upkeepText.text = levelData?.Upkeep.ToString("0.#") ?? string.Empty;
            effectsText.text = levelData?.Description ?? string.Empty;
        }

        public override void Reset()
        {
            titleText.text = string.Empty;
            descriptionText.text = string.Empty;
            effectsText.text = string.Empty;
        }
    }
}