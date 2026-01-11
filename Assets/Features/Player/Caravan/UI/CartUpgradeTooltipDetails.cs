using Common.UI.Utility;
using Common.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Player.Caravan.UI
{
    public sealed class CartUpgradeTooltipDetails : MonoBehaviour
    {
        [SerializeField, Required]
        private Image tierIcon;

        [SerializeField, Required]
        private TMP_Text tierText, slotText, speedText, upkeepText;

        [SerializeField, Required]
        private TMP_Text slotDiffText, speedDiffText, upkeepDiffText;

        public void SetUp(Sprite icon, int level, int slotCount, float speed, float upkeep)
        {
            tierIcon.sprite = icon;
            tierText.text = $"Level {level.ToRomanNumeral()}";
            slotText.text = $"Slots: {slotCount}";
            speedText.text = $"Speed: {speed:0.#}";
            upkeepText.text = $"Upkeep: {upkeep.Sign()}{upkeep:0.#}/day";

            slotDiffText.gameObject.SetActive(false);
            speedDiffText.gameObject.SetActive(false);
            upkeepDiffText.gameObject.SetActive(false);
        }

        public void SetDiffs(int slotDiff, float speedDiff, float upkeepDiff)
        {
            slotDiffText.text = $"{slotDiff.Sign()}{slotDiff}".WithStyle(Style.Good);
            speedDiffText.text = $"{speedDiff.Sign()}{speedDiff}".WithStyle(Style.Good);
            upkeepDiffText.text = $"{upkeepDiff.Sign()}{upkeepDiff}".WithStyle(Style.Bad);

            slotDiffText.gameObject.SetActive(true);
            speedDiffText.gameObject.SetActive(true);
            upkeepDiffText.gameObject.SetActive(true);
        }
    }
}