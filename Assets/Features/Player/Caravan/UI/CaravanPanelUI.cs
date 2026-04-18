using System.Collections.Generic;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI;
using Common.UI.Elements.Cells;
using Common.UI.Elements.Panels;
using Common.UI.Popups;
using Common.UI.Tooltips;
using Features.Player.Caravan.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.Player.Caravan.UI
{
    public sealed class CaravanPanelUI : DynamicPanel, IPointerClickHandler
    {
        [SerializeField, Required]
        private TMP_Text moveSpeedText, upkeepText;

        [SerializeField, Required]
        private ModifiableTooltipHandler moveSpeedTooltip, upkeepTooltip;

        [SerializeField]
        private List<CartUI> cartUis;

        private CaravanSlotService _slotService;
        private UIBridgeService _uiBridgeService;
        private CaravanManager _caravanManager;

        public void Setup()
        {
            _uiBridgeService = GameplayContext.Instance.Services.UIBridgeService;
            _caravanManager = GameplayContext.Instance.Model.Player.CaravanManager;
            _slotService = GameplayContext.Instance.Services.CaravanSlotService;

            // TODO - STYLE: it's not so nice to have a random business logic class in here.
            //   This would have to be in the view model layer. Probably should be system.
            var caravanUpgrader = new CaravanUpgrader();

            for (var i = 0; i < _caravanManager.Carts.Count; i++)
            {
                var cartId = i;
                cartUis[i].Bind(
                    _caravanManager.Carts[i],
                    i,
                    () => caravanUpgrader.RequestUpgrade(cartId),
                    () => caravanUpgrader.RequestUpgrade(_caravanManager.UnlockedCartCount));
            }

            _caravanManager.MoveSpeed.Observe(OnMoveSpeedChanged);
            _caravanManager.Upkeep.Observe(OnUpkeepChanged);

            moveSpeedTooltip.SetData(_caravanManager.MoveSpeed);
            upkeepTooltip.SetData(_caravanManager.Upkeep);
        }

        // background click should close popups
        public void OnPointerClick(PointerEventData eventData)
        {
            PopupManager.Instance.HideActive();
        }

        public InventoryCell GetCell(Good good)
        {
            var slot = _slotService.GetSlotForGood(good);

            if (slot == null)
                return null;

            var (cartIndex, slotIndex) = slot.Value;
            return cartUis[cartIndex].GetInventoryCell(slotIndex);
        }

        public MonoBehaviour GetUpgradeButton(int cartIndex)
        {
            return cartUis[cartIndex].UpgradeButton;
        }

        protected override void OnOpen()
        {
            _uiBridgeService.OpenPanelFromUI(UIPanel.Caravan);
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
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
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepText.text = upkeep.ToString("0.##");
        }
    }
}