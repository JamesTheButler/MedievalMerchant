using Common.UI.Popups;
using Common.UI.Tooltips;
using Common.Utility;
using Features.Towns.UI.Inventory;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Towns.UI
{
    public sealed class TownUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField, Required]
        private Button upgradeButton;

        [SerializeField, Required]
        private SimpleTooltipHandler upgradeButtonTooltip;

        [SerializeField, Required]
        private TownProductionPanel productionPanel;

        [SerializeField, Required]
        private TownInventoryPanel inventoryPanel;

        private Town _town;
        private TownUISection[] _sections;

        public void Initialize()
        {
            _sections = GetComponentsInChildren<TownUISection>();

            foreach (var section in _sections)
            {
                section.Initialize();
            }

            productionPanel.Initialize();
            inventoryPanel.Initialize();
        }

        public void Bind(Town town)
        {
            if (_town == town)
                return;

            Unbind();

            if (town == null) return;


            foreach (var section in _sections)
            {
                section.Bind(town);
            }

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
            _town.DevelopmentManager.Upgrade();
        }

        private void BindTown(Town town)
        {
            if (_town != town)
            {
                Unbind();
            }

            _town = town;

            productionPanel.Bind(_town);
            inventoryPanel.Bind(_town);

            _town.DevelopmentManager.DevelopmentScore.Observe(OnDevelopmentChanged);
        }

        private void OnDevelopmentChanged(float developmentScore)
        {
            var isButtonEnabled = developmentScore.IsApproximately(100f);
            upgradeButton.interactable = isButtonEnabled;
            upgradeButtonTooltip.SetEnabled(!isButtonEnabled);
        }

        public void Unbind()
        {
            if (_town == null)
                return;

            _town.DevelopmentManager.DevelopmentScore.StopObserving(OnDevelopmentChanged);

            foreach (var section in _sections)
            {
                section.Unbind(_town);
            }

            productionPanel.Unbind();
            inventoryPanel.Unbind();

            _town = null;
        }

        // background click should close popups
        public void OnPointerClick(PointerEventData eventData)
        {
            PopupManager.Instance.HideActive();
        }
    }
}