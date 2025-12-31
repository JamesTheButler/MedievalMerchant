using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Common.UI;
using Common.UI.Elements;
using Common.UI.Popups;
using Common.UI.Tooltips;
using Features.Player.Caravan.Logic;
using Features.Trade;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Features.Player.Caravan.UI
{
    public sealed class CaravanPanelUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private UnityEvent<InventoryCell, TradeType> inventoryCellClicked;

        [SerializeField, Required]
        private TMP_Text moveSpeedText, upkeepText;

        [SerializeField, Required]
        private ModifiableTooltipHandler moveSpeedTooltip, upkeepTooltip;

        [SerializeField]
        private List<CartUI> cartUis;

        private readonly HashSet<InventoryCell> _freeCells = new();
        private readonly Dictionary<Good, InventoryCell> _occupiedCells = new();

        private CaravanManager _caravanManager;
        private Inventory.Inventory _playerInventory;
        private UIBridgeService _uiBridgeService;

        public void Setup(CaravanManager caravanManager)
        {
            _uiBridgeService = GameplayContext.Instance.Services.UIBridgeService;
            _caravanManager = caravanManager;

            // TODO - STYLE: it's not so nice to have a random business logic class in here.
            //   This would have to be in the view model layer. Probably should be system.
            var caravanUpgrader = new CaravanUpgrader();

            for (var i = 0; i < caravanManager.Carts.Count; i++)
            {
                var cartId = i;
                cartUis[i].Bind(
                    caravanManager.Carts[i],
                    () => caravanUpgrader.RequestUpgrade(cartId),
                    () => caravanUpgrader.RequestUpgrade(caravanManager.UnlockedCartCount),
                    OnCellAdded);
                cartUis[i].OnCellClicked += OnCellClicked;
            }

            caravanManager.MoveSpeed.Observe(OnMoveSpeedChanged);
            caravanManager.Upkeep.Observe(OnUpkeepChanged);

            moveSpeedTooltip.SetData(caravanManager.MoveSpeed);
            upkeepTooltip.SetData(caravanManager.Upkeep);
        }

        public void Toggle(bool isOn)
        {
            if (isOn)
            {
                _uiBridgeService.OpenPanelFromUI(UIPanel.Caravan);
            }

            gameObject.SetActive(isOn);
        }

        public void UpdateGood(Good good, int amount)
        {
            if (_occupiedCells.ContainsKey(good))
            {
                UpdateExistingGood(good, amount);
            }
            else
            {
                AddNewGood(good, amount);
            }
        }

        private void OnDestroy()
        {
            foreach (var cartUI in cartUis)
            {
                cartUI.Unbind();
            }

            _caravanManager.MoveSpeed.StopObserving(OnMoveSpeedChanged);
            _caravanManager.Upkeep.StopObserving(OnUpkeepChanged);
        }

        private void OnCellClicked(InventoryCell cell)
        {
            inventoryCellClicked.Invoke(cell, TradeType.Sell);
        }

        private void AddNewGood(Good good, int amount)
        {
            if (amount <= 0)
                return;

            var cell = _freeCells.FirstOrDefault();
            if (cell == null)
            {
                Debug.LogError("No more free inventory cells found!");
                return;
            }

            _occupiedCells.Add(good, cell);
            cell.Update(good, amount);
            _freeCells.Remove(cell);
        }

        private void UpdateExistingGood(Good good, int amount)
        {
            var cell = _occupiedCells[good];

            if (amount > 0)
            {
                cell.SetAmount(amount);
            }
            else
            {
                cell.Reset();
                _occupiedCells.Remove(good);
                _freeCells.Add(cell);
            }
        }

        private void OnCellAdded(InventoryCell cell)
        {
            _freeCells.Add(cell);
        }

        private void OnMoveSpeedChanged(float moveSpeed)
        {
            moveSpeedText.text = moveSpeed.ToString("0.##");
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepText.text = upkeep.ToString("0.##");
        }

        // background click should close popups
        public void OnPointerClick(PointerEventData eventData)
        {
            PopupManager.Instance.HideActive();
        }
    }
}