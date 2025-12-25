using Common.Infrastructure;
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

        private void Start()
        {
            _retinueModel = GameplayContext.Instance.Model.Player.RetinueModel;
            _retinueModel.Upkeep.Observe(OnUpkeepChanged);
            upkeepTooltip.SetData(_retinueModel.Upkeep);
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepText.text = upkeep.ToString("0.#");
        }

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}