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

        private RetinueManager _retinueManager;

        private void Start()
        {
            _retinueManager = GameplayContext.Instance.Model.Player.RetinueManager;
            _retinueManager.Upkeep.Observe(OnUpkeepChanged);
            upkeepTooltip.SetData(_retinueManager.Upkeep);
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