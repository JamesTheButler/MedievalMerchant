using Common.Infrastructure.Gameplay;
using Common.Types;
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
        private Inventory.Inventory _playerInventory;

        public void TogglePanel(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            caravanPanelUI.Toggle();
        }

        private void Start()
        {
            _gameplayContext = GameplayContext.Instance;
            _playerInventory = _gameplayContext.Model.Player.Inventory;
            caravanPanelUI.Setup(_gameplayContext.Model.Player.CaravanManager);
            _gameplayContext.Selection.SelectedTown.Observe(OpenPanel, false);
            _playerInventory.GoodUpdated += OnGoodUpdated;
            foreach (var (good, amount) in _playerInventory.Goods)
            {
                caravanPanelUI.UpdateGood(good, amount);
            }
        }

        private void OnDestroy()
        {
            _gameplayContext.Selection.SelectedTown.StopObserving(OpenPanel);
            _playerInventory.GoodUpdated -= OnGoodUpdated;
        }

        private void OnGoodUpdated(Good good, int amount)
        {
            caravanPanelUI.UpdateGood(good, amount);
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