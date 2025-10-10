using AYellowpaper.SerializedCollections;
using Data;
using Data.Configuration;
using Data.Player;
using Data.Trade;
using NaughtyAttributes;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.InventoryUI
{
    public class PlayerInventoryUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private UnityEvent<InventoryCell> inventoryCellClicked;

        [SerializeField, Required]
        private TMP_Text fundsText;

        [SerializeField, SerializedDictionary("Tier", "Section")]
        private SerializedDictionary<Tier, InventorySection> inventorySections;


        private PlayerModel _player;
        private Inventory _playerInventory;
        private GoodsConfig _goodsConfig;

        public void Bind(PlayerModel player)
        {
            return;

            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            _player = player;
            _playerInventory = player.Inventory;

            SetUpUpgradeButtons();

            SetUpInventory();
        }

        public void Unbind()
        {
            return;

            _playerInventory.Funds.StopObserving(OnFundsChanged);

            foreach (var section in inventorySections.Values)
            {
                section.CellClicked -= InvokeCellClicked;
                section.CleanUp();
            }
        }

        private void SetUpInventory()
        {
            _playerInventory.Funds.Observe(OnFundsChanged);
        }

        private void SetUpUpgradeButtons()
        {
            foreach (var section in inventorySections.Values)
            {
                section.Initialize();
                section.CellClicked += InvokeCellClicked;
            }
        }

        private void OnFundsChanged(float funds)
        {
            fundsText.text = funds.ToString("N0");
        }

        private void InvokeCellClicked(InventoryCell cell)
        {
            inventoryCellClicked?.Invoke(cell);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PopupManager.Instance.HideActive();
        }
    }
}