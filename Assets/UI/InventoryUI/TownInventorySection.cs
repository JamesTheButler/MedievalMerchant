using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Configuration;
using Data.Towns;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public sealed class TownInventorySection : MonoBehaviour
    {
        [SerializeField]
        private Tier tier;

        [SerializeField]
        private Image tierIcon;

        private Town _town;
        private TierIconConfig _tierIconConfig;
        private GoodsConfig _goodsConfig;

        private readonly Dictionary<Good, InventoryCellBase> _cells = new();
        private readonly List<ProductionCell> _productionCells = new();
        private readonly List<InventoryCell> _inventoryCells = new();

        private void Start()
        {
            _tierIconConfig = ConfigurationManager.Instance.TierIconConfig;
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;

            tierIcon.sprite = _tierIconConfig.Icons[tier];

            GatherCells();
        }

        public void Bind(Town town)
        {
            _town = town;
            _town.ProductionAdded += OnProductionAdded;
        }

        private void OnProductionAdded(Good good)
        {
            // TODO: if a town has a good in it's regular inventory and then starts producing it, we need to update some cells.
            Debug.LogError($"{nameof(TownInventorySection)}.{nameof(OnProductionAdded)} not implemented yet");
        }

        private void GatherCells()
        {
            _productionCells.AddRange(GetComponentsInChildren<ProductionCell>());
            _inventoryCells.AddRange(GetComponentsInChildren<InventoryCell>());
        }

        public void UpdateGood(Good good, int amount)
        {
            if (_goodsConfig.ConfigData[good].Tier != tier) return;

            if (_cells.TryGetValue(good, out var cell))
            {
                cell.SetAmount(amount);
            }
            else
            {
                // TODO: check this case
                if (_town is null) 
                    return;
                // try to find free cell
                var isProduced = _town.Production.Contains(good);
                IEnumerable<InventoryCellBase> cellList = isProduced ? _productionCells : _inventoryCells;
                var freeCell = cellList.FirstOrDefault(potentiallyFreeCell => !potentiallyFreeCell.HasGood());
                if (freeCell == null)
                {
                    Debug.LogWarning($"There is no free cell for {good}.");
                    return;
                }

                _cells.Add(good, freeCell);
                freeCell.Update(good, amount);
            }
        }

        public void Clear()
        {
            _town = null;

            foreach (var cell in _cells)
            {
                cell.Value.SetAmount(0);
                cell.Value.SetGood(null);
            }
        }
    }
}