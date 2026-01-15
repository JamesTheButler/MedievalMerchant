using Common.Infrastructure.Gameplay;
using Common.UI;
using Common.UI.Popups;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.Towns.UI
{
    public sealed class TownUI : MonoBehaviour, IPointerClickHandler
    {
        private Town _town;
        private TownUISection[] _sections;
        private UIBridgeService _uiBridgeService;

        public void Initialize()
        {
            _uiBridgeService = GameplayContext.Instance.Services.UIBridgeService;
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
            _uiBridgeService.OpenPanelFromUI(UIPanel.Town);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void BindTown(Town town)
        {
            if (_town != town)
            {
                Unbind();
            }

            _town = town;
        }

        public void Unbind()
        {
            if (_town == null)
                return;

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