using System.Collections.Generic;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements.Cells;
using Common.UI.Elements.Panels;
using Features.Player.Caravan.Logic;
using UnityEngine;

namespace Features.Player.Caravan.UI
{
    public sealed class CaravanInventoryUI : DynamicPanel
    {
        [SerializeField]
        private List<CartInventoryUI> cartUis;

        private CaravanManager _caravanManager;
        private CaravanSlotService _slotService;

        public override void Initialize()
        {
            _caravanManager = GameplayContext.Instance.Model.Player.CaravanManager;
            _slotService = GameplayContext.Instance.Services.CaravanSlotService;

            for (var i = 0; i < _caravanManager.Carts.Count; i++)
            {
                cartUis[i].Bind(_caravanManager.Carts[i], i);
            }
        }

        public override void CleanUp()
        {
            foreach (var cartUI in cartUis)
            {
                cartUI.Unbind();
            }
        }

        public InventoryCell GetCell(Good good)
        {
            var slot = _slotService.GetSlotForGood(good);

            if (slot == null)
                return null;

            var (cartIndex, slotIndex) = slot.Value;
            return cartUis[cartIndex].GetInventoryCell(slotIndex);
        }

        protected override void OnOpen() { }
        protected override void OnClose() { }
    }
}