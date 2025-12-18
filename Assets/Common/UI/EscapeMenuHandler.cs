using Common.Infrastructure;
using Common.UI.Popups;
using Features.Ticking;
using UnityEngine;

namespace Common.UI
{
    public class EscapeMenuHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject escapeMenuRoot;

        private TickingService _tickingService;

        private bool IsActive => escapeMenuRoot.activeSelf;

        public void OpenMenu()
        {
            ToggleEscMenu(true);
        }

        public void OnCancel()
        {
            // TODO - STYLE: this is a bit hacky
            if (PopupManager.Instance.HasActivePopup)
            {
                PopupManager.Instance.HideActive();
                return;
            }

            ToggleEscMenu(!IsActive);
        }

        private void Start()
        {
            _tickingService = GameplayContext.Instance.Services.TickingService;
            ToggleEscMenu(false);
        }
        private void ToggleEscMenu(bool isActive)
        {
            escapeMenuRoot.SetActive(isActive);
            if (isActive)
            {
                _tickingService.Pause();
            }
            else
            {
                _tickingService.Resume();
            }
        }
    }
}