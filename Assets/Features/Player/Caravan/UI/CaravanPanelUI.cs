using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Elements;
using Common.UI.Tooltips;
using Features.Player.Caravan.Logic;
using Features.Towns;
using Features.Trade;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Player.Caravan.UI
{
    public sealed class CaravanPanelUI : MonoBehaviour
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

        private void Start()
        {
            _caravanManager = GameplayContext.Instance.Model.Player.CaravanManager;
            _playerInventory = GameplayContext.Instance.Model.Player.Inventory;

            GameplayContext.Instance.Selection.TownSelected += OnTownSelected;

            // TODO - STYLE: it's not so nice to have a random business logic class in here.
            //   This would have to be in the view model layer.
            var caravanUpgrader = new CaravanUpgrader();

            for (var i = 0; i < _caravanManager.Carts.Count; i++)
            {
                var cartId = i;
                cartUis[i].Bind(
                    _caravanManager.Carts[i],
                    () => caravanUpgrader.RequestUpgrade(cartId),
                    () => caravanUpgrader.RequestUpgrade(_caravanManager.UnlockedCartCount),
                    OnCellAdded);
                cartUis[i].OnCellClicked += OnCellClicked;
            }

            _caravanManager.MoveSpeed.Observe(OnMoveSpeedChanged);
            _caravanManager.Upkeep.Observe(OnUpkeepChanged);

            moveSpeedTooltip.SetData(_caravanManager.MoveSpeed);
            upkeepTooltip.SetData(_caravanManager.Upkeep);

            _playerInventory.GoodUpdated += OnGoodAdded;
        }

        private void OnTownSelected(Town _)
        {
            Toggle(true);
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

        public void Toggle()
        {
            Toggle(!gameObject.activeSelf);
        }

        private void Toggle(bool isOn)
        {
            gameObject.SetActive(isOn);
        }

        private void OnCellClicked(InventoryCell cell)
        {
            inventoryCellClicked.Invoke(cell, TradeType.Sell);
        }

        private void OnGoodAdded(Good good, int amount)
        {
            if (_occupiedCells.ContainsKey(good))
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
            else
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
    }
}