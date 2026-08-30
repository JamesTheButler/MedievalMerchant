using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI;
using Common.UI.Elements.Panels;
using Common.UI.Popups;
using Common.UI.Tooltips;
using Features.Player.Caravan.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Player.Caravan.UI
{
    public sealed class CaravanPanelUI : DynamicPanel, IPointerClickHandler
    {
        [SerializeField, Required]
        private RectTransform rootTransform;

        [SerializeField, Required]
        private TMP_Text moveSpeedText, upkeepText;

        [SerializeField, Required]
        private ModifiableTooltipHandler moveSpeedTooltip, upkeepTooltip;

        private CaravanManager _caravanManager;
        private UIBridgeService _uiBridgeService;

        private readonly Bindings _bindings = new();

        public override void Initialize()
        {
            _uiBridgeService = GameplayContext.Instance.Services.UIBridgeService;
            _caravanManager = GameplayContext.Instance.Model.Player.CaravanManager;

            _bindings.Track(
                _caravanManager.MoveSpeed.Observe(OnMoveSpeedChanged),
                _caravanManager.Upkeep.Observe(OnUpkeepChanged),
                _caravanManager.CartUnlocked.Observe(OnCartCountChanged)
            );

            moveSpeedTooltip.SetData(_caravanManager.MoveSpeed);
            upkeepTooltip.SetData(_caravanManager.Upkeep);
        }

        // background click should close popups
        public void OnPointerClick(PointerEventData eventData)
        {
            PopupManager.Instance.HideActive();
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

        public override void CleanUp()
        {
            _bindings.Unbind();
        }

        private void OnMoveSpeedChanged(float moveSpeed)
        {
            moveSpeedText.text = moveSpeed.ToString("0.##");
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepText.text = upkeep.ToString("0.##");
        }

        private void OnCartCountChanged()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rootTransform);
        }
    }
}