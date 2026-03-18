using Common.Config;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Tooltips;
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

        private TierResources _tierResources;
        private ReputationResources _reputationResources;

        public override void Initialize()
        {
            _tierResources = ResourceManager.Instance.TierResources;
            _reputationResources = ResourceManager.Instance.ReputationResources;
        }

        public override void CleanUp() { }

        public override void Bind(Town town)
        {
            flagIcon.SetFlag(town.FlagInfo);
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