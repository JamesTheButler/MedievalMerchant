using Common.Infrastructure.Gameplay;
using Features.Towns;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Player.Caravan.UI
{
    public sealed class CaravanPanelUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private CaravanPanelUI caravanPanelUI;

        private GameplayContext _gameplayContext;

        public void TogglePanel(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            caravanPanelUI.Toggle();
        }

        private void Start()
        {
            _gameplayContext = GameplayContext.Instance;
            _gameplayContext.Selection.SelectedTown.Observe(OpenPanel, false);
        }

        private void OnDestroy()
        {
            _gameplayContext.Selection.SelectedTown.StopObserving(OpenPanel);
        }

        private void OpenPanel(Town town)
        {
            if (town == null)
            {
                caravanPanelUI.Close();
            }
            else
            {
                caravanPanelUI.Open();
            }
        }
    }
}
