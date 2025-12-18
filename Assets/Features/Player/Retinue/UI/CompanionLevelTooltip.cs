using Common.Infrastructure;
using Common.UI.Tooltips;
using Features.Player.Retinue.Config;
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
        private TMP_Text levelText, priceText, effectsText, lockedText;

        [SerializeField, Required]
        private GameObject priceGroup, lockedGroup;

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

            priceGroup.gameObject.SetActive(data.State != CompanionLevelUI.State.Unlocked);
            lockedGroup.gameObject.SetActive(data.State == CompanionLevelUI.State.Locked);

            levelText.text = $"{companionData.Name} lvl. {data.Level}";

            if (levelData == null)
            {
                Debug.LogError($"Level data is null here. (Level {data.Level}");
                return;
            }

            priceText.text = $"{levelData.Cost:0.##}";
            effectsText.text = levelData.Description;

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