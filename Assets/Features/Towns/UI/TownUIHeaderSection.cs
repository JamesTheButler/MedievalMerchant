using Common.Config;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Tooltips;
using Features.Localization.Data;
using Features.Towns.Flags.UI;
using Features.Towns.Reputation.Data;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.UI
{
    public sealed class TownUIHeaderSection : TownUISection
    {
        [SerializeField, Required]
        private TMP_Text nameText, descriptorText, reputationText, fundsText, fundsChangeText;

        [SerializeField, Required]
        private RectTransform regionIconGroup;

        [SerializeField, Required]
        private ModifiableTooltipHandler fundsChangeTooltip;

        [SerializeField, Required]
        private FlagRenderer flagIcon;

        [SerializeField, Required]
        private Image tierIcon, reputationIcon;

        [SerializeField, Required]
        private SimpleIconTooltipHandler tierIconTooltip, flagIconTooltip;

        private TierResources _tierResources;
        private RegionResources _regionResources;
        private ReputationResources _reputationResources;
        private LocalizationResources _localizationResources;

        public override void Initialize()
        {
            _tierResources = ResourceManager.Instance.TierResources;
            _regionResources = ResourceManager.Instance.RegionResources;
            _reputationResources = ResourceManager.Instance.ReputationResources;
            _localizationResources = ResourceManager.Instance.LocalizationResources;
        }

        public override void CleanUp() { }

        public override void Bind(Town town)
        {
            flagIcon.SetFlag(town.FlagInfo);
            var regionResource = _regionResources.Data[town.MainRegion];
            var tooltipData = new SimpleIconTooltip.Data(regionResource.Icon, regionResource.Name);
            flagIconTooltip.SetData(tooltipData);
            nameText.text = town.Name;

            fundsChangeTooltip.SetData(town.FundsChange);

            town.Tier.Observe(OnTierChanged);
            town.Descriptor.Observe(OnDescriptorChanged);
            town.ReputationModel.Reputation.Observe(OnReputationChanged);
            town.Inventory.Funds.Observe(OnFundsChanged);
            town.FundsChange.Observe(OnFundsChangeChanged);
        }

        public override void Unbind(Town town)
        {
            town.Tier.StopObserving(OnTierChanged);
            town.Descriptor.StopObserving(OnDescriptorChanged);
            town.ReputationModel.Reputation.StopObserving(OnReputationChanged);
            town.Inventory.Funds.StopObserving(OnFundsChanged);
            town.FundsChange.StopObserving(OnFundsChangeChanged);
        }

        private void OnTierChanged(Tier tier)
        {
            var tierSprite = _tierResources.Icons[tier];
            tierIcon.sprite = tierSprite;
            var tooltipData = new SimpleIconTooltip.Data(tierSprite, _localizationResources.Tier(tier));
            tierIconTooltip.SetData(tooltipData);
        }

        private void OnDescriptorChanged(string descriptor)
        {
            descriptorText.text = descriptor;
        }

        private void OnReputationChanged(float newReputation)
        {
            reputationText.text = $"{newReputation:0.#}";
            var isHappy = newReputation >= 0;
            reputationIcon.sprite = isHappy ? _reputationResources.HappyIcon : _reputationResources.UnhappyIcon;
        }

        private void OnFundsChanged(float funds)
        {
            fundsText.text = $"{funds:0.#}";
        }

        private void OnFundsChangeChanged(float fundsChange)
        {
            fundsChangeText.text = $"{fundsChange:+0.0;-0.0;0.0}";
        }
    }
}