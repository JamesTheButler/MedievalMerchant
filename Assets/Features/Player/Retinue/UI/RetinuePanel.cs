using Common.Infrastructure.Gameplay;
using Common.UI;
using Common.UI.Elements;
using Common.UI.Tooltips;
using Features.Player.Retinue.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinuePanel : DynamicPanel
    {
        [SerializeField, Required]
        private TMP_Text upkeepText;

        [SerializeField, Required]
        private ModifiableTooltipHandler upkeepTooltip;

        private RetinueModel _retinueModel;
        private UIBridgeService _uiBridgeService;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _retinueModel = GameplayContext.Instance.Model.Player.RetinueModel;
            _uiBridgeService = GameplayContext.Instance.Services.UIBridgeService;
            _retinueModel.Upkeep.Observe(OnUpkeepChanged);
            upkeepTooltip.SetData(_retinueModel.Upkeep);
        }

        private void Start()
        {
            Close();
        }

        protected override void OnOpen()
        {
            _uiBridgeService.OpenPanelFromUI(UIPanel.Retinue);
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepText.text = upkeep.ToString("0.#");
        }
    }
}