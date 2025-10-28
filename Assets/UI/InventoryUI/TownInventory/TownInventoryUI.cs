using Common;
using Common.Types;
using Common.UI;
using Features.Inventory;
using Features.Towns;
using Features.Towns.Development.UI.DevelopmentGauge;
using Features.Towns.Flags.UI;
using NaughtyAttributes;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InventoryUI.TownInventory
{
    public sealed class TownInventoryUI : MonoBehaviour, IPointerClickHandler
    {
        [Header("Events")]
        [SerializeField]
        private UnityEvent<Town> travelButtonClicked;

        [Header("Header UI Elements")]
        [SerializeField, Required]
        private TMP_Text townNameText;

        [SerializeField, Required]
        private FlagUI flagUI;

        [SerializeField, Required]
        private DevelopmentGauge developmentGauge;

        [SerializeField, Required]
        private Button upgradeButton;

        [SerializeField, Required]
        private SimpleTooltipHandler upgradeButtonTooltip;

        [Header("Inventory UI Elements")]
        [SerializeField, Required]
        private TMP_Text fundsText;

        [SerializeField, Required]
        private ModifiableTooltipHandler fundsTooltip;

        [SerializeField, Required]
        private TownProductionPanel productionPanel;

        [SerializeField, Required]
        private TownInventoryPanel inventoryPanel;

        private Town _town;
        private Inventory _inventory;

        public void Initialize()
        {
            productionPanel.Initialize();
            inventoryPanel.Initialize();
        }

        public void Bind(Town town)
        {
            if (_town == town)
                return;

            Unbind();

            if (town == null) return;

            BindTown(town);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Upgrade()
        {
            _town.Upgrade();
        }

        public void TravelHere()
        {
            travelButtonClicked?.Invoke(_town);
        }

        private void BindTown(Town town)
        {
            _town = town;

            flagUI.SetFlag(_town.FlagInfo);
            productionPanel.Bind(_town);
            inventoryPanel.Bind(_town);

            BindInventory(_town.Inventory);

            _town.DevelopmentManager.DevelopmentScore.Observe(OnDevelopmentChanged);
            _town.Tier.Observe(TownUpgrade);
            fundsTooltip.SetData(_town.FundsChange);

            RefreshTownName(_town.Tier);

            developmentGauge.Bind(_town);
        }

        private void OnDevelopmentChanged(float developmentScore)
        {
            var isButtonEnabled = developmentScore.IsApproximately(100f);
            Debug.LogError($"is enabled {isButtonEnabled}, dev score {developmentScore}");
            upgradeButton.interactable = isButtonEnabled;
            upgradeButtonTooltip.SetEnabled(!isButtonEnabled);
        }

        private void BindInventory(Inventory inventory)
        {
            inventory.Funds.Observe(OnFundsUpdated);

            _inventory = inventory;
        }

        public void Unbind()
        {
            UnbindTown();
            UnbindInventory();
        }

        private void UnbindTown()
        {
            if (_town == null) return;

            _town.DevelopmentManager.DevelopmentScore.StopObserving(OnDevelopmentChanged);
            _town.Tier.StopObserving(TownUpgrade);
            developmentGauge.Unbind();
            fundsTooltip.SetData(_town.FundsChange);

            _town = null;
        }

        private void UnbindInventory()
        {
            productionPanel.Unbind();
            inventoryPanel.Unbind();

            fundsText.text = "0";

            if (_inventory == null) return;

            _inventory.Funds.StopObserving(OnFundsUpdated);
            _inventory = null;
        }

        private void RefreshTownName(Tier tier)
        {
            townNameText.text = $"{_town.Name} ({tier.ToRomanNumeral()})";
        }

        private void TownUpgrade(Tier tier)
        {
            RefreshTownName(tier);
        }

        private void OnFundsUpdated(float funds)
        {
            fundsText.text = funds.ToString("N0");
        }

        // background click should close popups
        public void OnPointerClick(PointerEventData eventData)
        {
            PopupManager.Instance.HideActive();
        }
    }
}