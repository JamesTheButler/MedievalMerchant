using Common;
using Data;
using Data.Towns;
using Data.Trade;
using NaughtyAttributes;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
        private DevelopmentGauge developmentGauge;

        [Header("Inventory UI Elements")]
        [SerializeField, Required]
        private TMP_Text fundsText;

        [SerializeField, Required]
        private TownProductionPanel productionPanel;

        [SerializeField, Required]
        private TownInventoryPanel inventoryPanel;

        private Town _town;
        private DevelopmentManager _developmentManager;
        private Inventory _inventory;

        public void Initialize()
        {
            productionPanel.Initialize();
            inventoryPanel.Initialize();
        }

        public void Bind(Town town)
        {
            Unbind();

            if (town == null) return;

            BindTown(town);

            // makes sure that the UI is properly inflated after dynamic inventory creation
            Canvas.ForceUpdateCanvases();
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

            productionPanel.Bind(_town);
            inventoryPanel.Bind(_town);

            BindInventory(_town.Inventory);

            _town.Tier.Observe(TownUpgrade);

            RefreshTownName(_town.Tier);

            developmentGauge.Bind(_developmentManager);
        }


        private void BindInventory(Inventory inventory)
        {
            inventory.Funds.Observe(OnFundsUpdated);

            _inventory = inventory;
            _developmentManager = _town.DevelopmentManager;
        }

        public void Unbind()
        {
            UnbindTown();
            UnbindInventory();
        }

        private void UnbindTown()
        {
            if (_town == null) return;

            _town.Tier.StopObserving(TownUpgrade);
            developmentGauge.Unbind();

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

        private void OnFundsUpdated(int funds)
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