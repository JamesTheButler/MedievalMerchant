using Common;
using Common.UI;
using Features.Player.Retinue.Config;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class CompanionTooltip : TooltipBase<CompanionTooltip.Data>
    {
        public sealed record Data(CompanionType CompanionType, int Level, bool IsImplemented);

        [SerializeField, Required]
        private TMP_Text titleText, descriptionText, effectsText;

        private CompanionConfig _configData;

        private void Awake()
        {
            _configData = ConfigurationManager.Instance.CompanionConfig;
        }

        public override void SetData(Data data)
        {
            var companionData = _configData.Get(data.CompanionType);
            var levelData = companionData.GetLevelData(data.Level);

            var comingSoonSuffix = data.IsImplemented ? string.Empty : " - (coming soon)";
            titleText.text = $"{companionData.Name} (Level {data.Level}){comingSoonSuffix}";
            descriptionText.text = companionData.Description;

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