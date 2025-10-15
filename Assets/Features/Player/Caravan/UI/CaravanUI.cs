using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Types;
using Common.UI;
using Features.Player.Caravan.Logic;
using NaughtyAttributes;
using TMPro;
using UI;
using UI.InventoryUI;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Player.Caravan.UI
{
    public sealed class CaravanUI : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<InventoryCell> inventoryCellClicked;

        [SerializeField, Required]
        private TMP_Text moveSpeedText, upkeepText;

        [SerializeField, Required]
        private SimpleTooltipHandler moveSpeedTooltip, upkeepTooltip;

        [SerializeField]
        private List<CartUI> cartUis;

        private readonly HashSet<InventoryCell> _freeCells = new();
        private readonly Dictionary<Good, InventoryCell> _occupiedCells = new();

        private CaravanManager _caravanManager;
        private Inventory.Inventory _playerInventory;

        private void Start()
        {
            _caravanManager = Model.Instance.Player.CaravanManager;
            _playerInventory = Model.Instance.Player.Inventory;

            for (var i = 0; i < _caravanManager.Carts.Count; i++)
            {
                var cartId = i;
                cartUis[i].Bind(_caravanManager.Carts[i], () => _caravanManager.UpgradeCart(cartId), OnCellAdded);
                cartUis[i].OnCellClicked += inventoryCellClicked.Invoke;
            }

            _caravanManager.MoveSpeed.Observe(OnMoveSpeedChanged);
            _caravanManager.Upkeep.Observe(OnUpkeepChanged);

            _playerInventory.GoodUpdated += OnGoodAdded;
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

        private void OnDestroy()
        {
            foreach (var cartUI in cartUis)
            {
                cartUI.Unbind();
            }

            _caravanManager.MoveSpeed.StopObserving(OnMoveSpeedChanged);
            _caravanManager.Upkeep.StopObserving(OnUpkeepChanged);
        }

        private void OnMoveSpeedChanged(float moveSpeed)
        {
            moveSpeedText.text = moveSpeed.ToString("0.##");
            moveSpeedTooltip.SetTooltip(_caravanManager.MoveSpeed.ToString());
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepText.text = upkeep.ToString("0.##");
            upkeepTooltip.SetTooltip(_caravanManager.Upkeep.ToString());
        }
    }
}