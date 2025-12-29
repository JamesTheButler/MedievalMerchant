using Common.UI.Popups;
using Common.UI.Tooltips;
using Common.Utility;
using Features.Towns.Development.UI.DevelopmentGauge;
using Features.Towns.Missions.UI;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Towns.UI.Inventory
{
    public sealed class TownInventoryUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField, Required]
        private DevelopmentGauge developmentGauge;

        [SerializeField, Required]
        private Button upgradeButton;

        [SerializeField, Required]
        private SimpleTooltipHandler upgradeButtonTooltip;

        [SerializeField, Required]
        private TownProductionPanel productionPanel;

        [SerializeField, Required]
        private TownInventoryPanel inventoryPanel;

        [SerializeField, Required]
        private MissionPanel missionPanel;

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
            missionPanel.Initialize();
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
            _town = town;

            productionPanel.Bind(_town);
            inventoryPanel.Bind(_town);
            missionPanel.Bind(_town.Missions);

            _town.DevelopmentManager.DevelopmentScore.Observe(OnDevelopmentChanged);

            developmentGauge.Bind(_town);
        }

        private void OnDevelopmentChanged(float developmentScore)
        {
            var isButtonEnabled = developmentScore.IsApproximately(100f);
            upgradeButton.interactable = isButtonEnabled;
            upgradeButtonTooltip.SetEnabled(!isButtonEnabled);
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
            developmentGauge.Unbind();
            missionPanel.Unbind(_town.Missions);

            _town = null;
        }

        private void UnbindInventory()
        {
            productionPanel.Unbind();
            inventoryPanel.Unbind();
        }

        // background click should close popups
        public void OnPointerClick(PointerEventData eventData)
        {
            PopupManager.Instance.HideActive();
        }
    }
}