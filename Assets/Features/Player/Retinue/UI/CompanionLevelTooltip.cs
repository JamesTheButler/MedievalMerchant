using System.Linq;
using Common.Infrastructure;
using Common.UI.Tooltips;
using Features.Localization.Data;
using Features.Player.Retinue.Config;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

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

        [SerializeField]
        private LocalizedString levelString;

        private CompanionConfig _configData;
        private CompanionResources _companionResources;
        private LocalizationResources _loc;

        protected override void Awake()
        {
            base.Awake();

            _configData = ConfigurationManager.Configurations.CompanionConfig;
            _loc = ResourceManager.Instance.LocalizationResources;
            _companionResources = ResourceManager.Instance.CompanionResources;
        }

        protected override void UpdateUI(Data data)
        {
            var companionData = _configData.Get(data.CompanionType);
            var levelData = companionData.GetLevelData(data.Level);

            priceGroup.gameObject.SetActive(data.State != CompanionLevelUI.State.Unlocked);
            lockedGroup.gameObject.SetActive(data.State == CompanionLevelUI.State.Locked);

            var companionName = _companionResources.Get(data.CompanionType).Name;
            levelText.text = _loc.Player.Companions.CompanionDisplayString(companionName, data.Level);

            if (levelData == null)
            {
                Debug.LogError($"Level data is null here. (Level {data.Level}");
                return;
            }

            var cost = companionData.MissionConfig.ConfigsPerLevel.ElementAtOrDefault(data.Level - 1)?.Cost;
            priceText.text = $"{cost:0.##}";
            effectsText.text = levelData.Description;
            upkeepText.text = _loc.PerDay(levelData.Upkeep.ToString("0.#"));
        }

        public override void Reset()
        {
            levelText.text = string.Empty;
            priceText.text = string.Empty;
            effectsText.text = string.Empty;
        }
    }
}