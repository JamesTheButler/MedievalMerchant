using System;
using AYellowpaper.SerializedCollections;
using Common;
using Common.Types;
using Features.Goods.Config;
using Features.Towns;
using UnityEngine;
using UnityEngine.Events;

namespace UI.InventoryUI.TownInventory
{
    public sealed class TownInventoryPanel : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<InventoryCellBase> inventoryCellClicked;

        [SerializeField, SerializedDictionary("Tier", "Section")]
        private SerializedDictionary<Tier, InventoryTierRow> rows;

        private readonly Lazy<GoodsConfig> _goodsConfig = new(() => ConfigurationManager.Instance.GoodsConfig);

        private Town _town;

        public void Initialize()
        {
            foreach (var row in rows.Values)
            {
                row.InventoryCellClicked += cell => inventoryCellClicked.Invoke(cell);
            }
        }

        public void Bind(Town town)
        {
            _town = town;

            BindTownTier();
            BindInventory();
        }

        private void BindInventory()
        {
            foreach (var (good, amount) in _town.Inventory.Goods)
            {
                UpdateGood(good, amount);
            }

            _town.Inventory.GoodUpdated += UpdateGood;
        }

        public void Unbind()
        {
            foreach (var row in rows.Values)
            {
                row.Reset();
            }

            HideAllRows();

            if (_town == null) return;
            _town.Tier.StopObserving(ShowRow);
            _town.Inventory.GoodUpdated -= UpdateGood;
            _town = null;
        }

        private void UpdateGood(Good good, int amount)
        {
            // ignore goods produced in this town. ProductionPanel handles that
            if (_town.ProductionManager.IsProduced(good))
                return;

            var goodTier = _goodsConfig.Value.ConfigData[good].Tier;
            rows[goodTier].UpdateGood(good, amount);
        }

        private void BindTownTier()
        {
            // don't invoke directly as we want to go through all tiers manually in the right order
            _town.Tier.Observe(ShowRow, false);
            for (var tier = Tier.Tier1; tier <= _town.Tier; tier++)
            {
                ShowRow(tier);
            }
        }

        private void HideAllRows()
        {
            foreach (var row in rows.Values)
            {
                row.gameObject.SetActive(false);
            }
        }

        private void ShowRow(Tier tier)
        {
            rows[tier].gameObject.SetActive(true);
        }
    }
}