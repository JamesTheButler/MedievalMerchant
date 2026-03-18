using Common.Infrastructure;
using Common.UI.Tooltips;
using Features.Localization.Data;
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
        private CompanionResources _companionResources;
        private CompanionLocalizationResources _loc;

        protected override void Awake()
        {
            base.Awake();

            _configData = ConfigurationManager.Configurations.CompanionConfig;
            _companionResources = ResourceManager.Instance.CompanionResources;
            _loc = ResourceManager.Instance.LocalizationResources.Player.Companions;
        }

        protected override void UpdateUI(Data data)
        {
            var companionData = _configData.Get(data.CompanionType);
            var levelData = companionData.GetLevelData(data.Level);

            var resource = _companionResources.Get(data.CompanionType);
            titleText.text = _loc.CompanionDisplayString(resource.Name, data.Level);
            descriptionText.text = resource.Description;

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