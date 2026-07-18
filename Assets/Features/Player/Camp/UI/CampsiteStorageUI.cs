using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI.Elements.Cells;
using Common.UI.Elements.Panels;
using Features.Player.Camp.Logic;
using Features.Player.Logic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Player.Camp.UI
{
    public sealed class CampsiteStorageUI : DynamicPanel
    {
        [SerializeField, Required]
        private InventoryCellContainer campStorageCells;

        [SerializeField]
        private LocalizedString notAtCampString;

        private CampsiteStorageService _storageService;
        private PlayerModel _player;
        private Logic.Camp _campsite;

        private readonly Bindings _bindings = new();

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _player = GameplayContext.Instance.Model.Player;
            _storageService = GameplayContext.Instance.Services.CampsiteStorageService;
            _campsite = GameplayContext.Instance.Model.Camp;

            _bindings.Track(
                _campsite.Inventory.GoodAmountChanged.Observe(campStorageCells.UpdateGood),
                campStorageCells.OnCellClicked.Observe(OnCellClicked)
            );
        }

        private void OnCellClicked(InventoryCell cell)
        {
            if (!_player.Location.IsAtCampsite())
            {
                cell.PostMessage(notAtCampString.GetLocalizedString());
                return;
            }

            if (cell.Good == null)
                return;

            var good = cell.Good!.Value;
            _storageService.TransferToPlayer(good, _campsite.Inventory.Get(good));
        }

        public void HandleCaravanCellClick(GoodCell cell)
        {
            if (!_player.Location.IsAtCampsite())
            {
                cell.PostMessage(notAtCampString.GetLocalizedString());
                return;
            }

            if (cell.Good == null)
                return;

            var good = cell.Good!.Value;
            _storageService.TransferToCamp(good, _player.Inventory.Get(good));
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