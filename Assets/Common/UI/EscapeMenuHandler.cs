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

        public void ToggleMenu()
        {
            ToggleEscMenu(!IsActive);
        }

        public void OpenMenu()
        {
            ToggleEscMenu(true);
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