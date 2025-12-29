using Common.Config;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Tooltips;
using Common.Utility;
using Features.Player.Logic;
using Features.Towns.Flags.UI;
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
        private GameObject inTownIndicator;

        [SerializeField, Required]
        private RectTransform regionIconGroup;

        [SerializeField, Required]
        private Button gotoButton;

        [SerializeField, Required]
        private SimpleTooltipHandler gotoButtonTooltip;

        [SerializeField, Required]
        private ModifiableTooltipHandler fundsChangeTooltip;

        [SerializeField, Required]
        private FlagUI flagIcon;

        [SerializeField, Required]
        private Image tierIcon;

        private PlayerModel _playerModel;
        private TierResources _tierResources;
        private Town _town;

        public override void Initialize()
        {
            _playerModel = GameplayContext.Instance.Model.Player;
            _tierResources = ResourceManager.Instance.TierResources;
        }

        public override void CleanUp() { }

        public override void Bind(Town town)
        {
            _town = town;
            flagIcon.SetFlag(town.FlagInfo);
            nameText.text = town.Name;

            _playerModel.Location.TownEntered += OnTownEntered;
            _playerModel.Location.TownExited += OnTownExited;
            OnTownEntered(_playerModel.Location.CurrentTown);

            // TODO: is this OK when we change the town??
            fundsChangeTooltip.SetData(town.FundsChange);
            
            town.Tier.Observe(OnTierChanged);
            town.Descriptor.Observe(OnDescriptorChanged);
            town.ReputationManager.Reputation.Observe(OnReputationChanged);
            town.Inventory.Funds.Observe(OnFundsChanged);
            town.FundsChange.Observe(OnFundsChangeChanged);
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

        private void OnReputationChanged(float reputation)
        {
            reputationText.text = $"{reputation:0.#}";
        }

        private void OnFundsChanged(float funds)
        {
            fundsText.text = $"{funds:0.#}";
        }

        private void OnFundsChangeChanged(float fundsChange)
        {
            fundsChangeText.text = $"{fundsChange.Sign()}{fundsChange:0.#} /day";
        }

        public override void Unbind(Town town)
        {
            _playerModel.Location.TownEntered -= OnTownEntered;
            _playerModel.Location.TownExited -= OnTownExited;
            
            town.Tier.StopObserving(OnTierChanged);
            town.Descriptor.StopObserving(OnDescriptorChanged);
            town.ReputationManager.Reputation.StopObserving(OnReputationChanged);
            town.Inventory.Funds.StopObserving(OnFundsChanged);
            town.FundsChange.StopObserving(OnFundsChangeChanged);
        }

        private void OnTownEntered(Town town)
        {
            SetInTown(town == _town);
        }

        private void OnTownExited(Town town)
        {
            SetInTown(false);
        }

        private void SetInTown(bool isPlayerInThisTown)
        {
            var tooltipContent = isPlayerInThisTown ? "You are here." : $"Travel to {_town.Name}";
            gotoButtonTooltip.SetData(tooltipContent);
            gotoButton.interactable = !isPlayerInThisTown;
            inTownIndicator.SetActive(isPlayerInThisTown);
        }
    }
}