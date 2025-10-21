using Common;
using Common.UI;
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
            bool IsUnlocked,
            bool IsUpgraded,
            bool IsImplemented);

        [SerializeField, Required]
        private TMP_Text levelText, priceText, effectsText, lockedText;

        [SerializeField, Required]
        private GameObject priceGroup, lockedGroup;

        private CompanionConfig _configData;

        private void Awake()
        {
            _configData = ConfigurationManager.Instance.CompanionConfig;
        }

        public override void SetData(Data data)
        {
            var companionData = _configData.Get(data.CompanionType);
            var levelData = companionData.GetLevelData(data.Level);

            priceGroup.gameObject.SetActive(!data.IsUpgraded);
            lockedGroup.gameObject.SetActive(!data.IsUnlocked);

            levelText.text = $"Level {data.Level}";

            if (levelData == null)
            {
                Debug.LogError($"Level data is null here. (Level {data.Level}");
                return;
            }

            priceText.text = $"{levelData.Cost:0.##}";
            effectsText.text = levelData.Description;


            lockedText.text = data.IsImplemented ? "Unlock previous levels first" : "(coming soon)";
        }

        public override void Reset()
        {
            levelText.text = string.Empty;
            priceText.text = string.Empty;
            effectsText.text = string.Empty;
        }
    }
}