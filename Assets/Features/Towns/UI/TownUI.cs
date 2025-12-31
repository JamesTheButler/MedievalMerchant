using Common.Infrastructure;
using Common.UI.Popups;
using Common.UI.Tooltips;
using Common.Utility;
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

        private Town _town;
        private TownUISection[] _sections;

        private UIEventService _uiEventService;
        
        public void Initialize()
        {
            _uiEventService = GameplayContext.Instance.Services.UIEventService;
            _sections = GetComponentsInChildren<TownUISection>();

            foreach (var section in _sections)
            {
                section.Initialize();
            }
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
            _uiEventService.OpenTownPanel();
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

            _town = null;
        }

        // background click should close popups
        public void OnPointerClick(PointerEventData eventData)
        {
            PopupManager.Instance.HideActive();
        }
    }
}