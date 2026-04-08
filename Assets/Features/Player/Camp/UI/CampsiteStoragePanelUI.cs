using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI.Elements.Cells;
using Common.UI.Elements.Panels;
using Features.Player.Camp.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Player.Camp.UI
{
    public sealed class CampsiteStoragePanelUI : DynamicPanel
    {
        [SerializeField, Required]
        private InventoryCellContainer inventoryCells;

        private CampsiteStorageService _storageService;
        private Logic.Camp _campsite;

        private readonly Bindings _bindings = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _storageService = GameplayContext.Instance.Services.CampsiteStorageService;
            _campsite = GameplayContext.Instance.Model.Camp;

            _bindings.Track(
                _campsite.Inventory.GoodAmountChanged.Observe(inventoryCells.UpdateGood),
                inventoryCells.OnCellClicked.Observe(OnCellClicked)
            );
        }

        private void OnCellClicked(InventoryCell cell)
        {
            if (cell.Good == null) return;

            var good = cell.Good!.Value;
            _storageService.TransferToCamp(good, _campsite.Inventory.Get(good));
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}