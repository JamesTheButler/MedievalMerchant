using Common.Infrastructure;
using Common.Types;
using Features.Towns;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Player.Caravan.UI
{
    public sealed class CaravanPanelUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private CaravanPanelUI caravanPanelUI;

        private GameplayContext _gameplayContext;
        private Inventory.Inventory _playerInventory;

        public void TogglePanel()
        {
            caravanPanelUI.Toggle(!caravanPanelUI.gameObject.activeSelf);
        }

        private void Start()
        {
            _gameplayContext = GameplayContext.Instance;
            _playerInventory = _gameplayContext.Model.Player.Inventory;
            caravanPanelUI.Setup(_gameplayContext.Model.Player.CaravanManager);
            _gameplayContext.Selection.TownSelected += OpenPanel;
            _playerInventory.GoodUpdated += OnGoodUpdated;
            foreach (var (good, amount) in _playerInventory.Goods)
            {
                caravanPanelUI.UpdateGood(good, amount);
            }
        }

        private void OnDestroy()
        {
            _gameplayContext.Selection.TownSelected -= OpenPanel;
            _playerInventory.GoodUpdated -= OnGoodUpdated;
        }

        private void OnGoodUpdated(Good good, int amount)
        {
            caravanPanelUI.UpdateGood(good, amount);
        }

        private void OpenPanel(Town town)
        {
            caravanPanelUI.Toggle(town != null);
        }
    }
}