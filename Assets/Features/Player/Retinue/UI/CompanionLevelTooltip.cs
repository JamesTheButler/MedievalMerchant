using System.Linq;
using Common.Infrastructure;
using Common.UI.Tooltips;
using Features.Localization.Data;
using Features.Player.Retinue.Config;
using Features.Player.Retinue.Config.Resources;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class CompanionLevelTooltip : TooltipBase<CompanionLevelTooltip.Data>
    {
        public sealed record Data(
            CompanionType CompanionType,
            int Level,
            CompanionLevelUI.State State);

        [SerializeField, Required]
        private GameObject upkeepLine;

        [SerializeField, Required]
        private TMP_Text levelText, priceText, effectsText, lockedText, upkeepText;

        [SerializeField, Required]
        private GameObject priceGroup, lockedGroup;

        private CompanionConfig _configData;
        private CompanionResources _companionResources;
        private CompanionLocalizationResources _loc;

        protected override void Awake()
        {
            base.Awake();

            _configData = ConfigurationManager.Configurations.CompanionConfig;
            _loc = ResourceManager.Instance.LocalizationResources.Player.Companions;
            _companionResources = ResourceManager.Instance.CompanionResources;
        }

        protected override void UpdateUI(Data data)
        {
            var companionData = _configData.Get(data.CompanionType);
            var levelData = companionData.GetLevelData(data.Level);

            priceGroup.gameObject.SetActive(data.State != CompanionLevelUI.State.Unlocked);
            lockedGroup.gameObject.SetActive(data.State == CompanionLevelUI.State.Locked);

            var companionName = _companionResources.Get(data.CompanionType).Name;
            levelText.text = _loc.CompanionDisplayString(companionName, data.Level);

            if (levelData == null)
            {
                Debug.LogError($"Level data is null here. (Level {data.Level}");
                return;
            }

            var cost = companionData.MissionConfig.ConfigsPerLevel.ElementAtOrDefault(data.Level - 1)?.Cost;
            priceText.text = $"{cost:0.##}";
            effectsText.text = levelData.Description;

            upkeepText.text = levelData.Upkeep.ToString("0.#");
            lockedText.text = companionData.IsImplemented ? "Unlock previous levels first" : "(coming soon)";
        }

        public override void Reset()
        {
            levelText.text = string.Empty;
            priceText.text = string.Empty;
            effectsText.text = string.Empty;
        }
    }
}