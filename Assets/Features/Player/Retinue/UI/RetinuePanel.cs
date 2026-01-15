using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.UI;
using Common.UI.Tooltips;
using Features.Player.Retinue.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Player.Retinue.UI
{
    public sealed class RetinuePanel : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text upkeepText;

        [SerializeField, Required]
        private ModifiableTooltipHandler upkeepTooltip;

        private RetinueModel _retinueModel;
        private UIBridgeService _uiBridgeService;

        private void Start()
        {
            _retinueModel = GameplayContext.Instance.Model.Player.RetinueModel;
            _uiBridgeService = GameplayContext.Instance.Services.UIBridgeService;
            _retinueModel.Upkeep.Observe(OnUpkeepChanged);
            upkeepTooltip.SetData(_retinueModel.Upkeep);
            Close();
        }

        public void Toggle()
        {
            if (gameObject.activeSelf)
                Close();
            else
            {
                Open();
            }
        }

        public void Open()
        {
            _uiBridgeService.OpenPanelFromUI(UIPanel.Retinue);
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepText.text = upkeep.ToString("0.#");
        }
    }
}