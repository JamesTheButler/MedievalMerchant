using Common.Infrastructure;
using Common.UI.Utility;
using Common.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
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

        [SerializeField]
        private LocalizedString cartLevelString;

        public void SetUp(Sprite icon, int level, int slotCount, float speed, float upkeep)
        {
            var loc = ResourceManager.Instance.LocalizationResources;
            tierIcon.sprite = icon;
            tierText.text = cartLevelString.GetLocalizedString(level.ToRomanNumeral());
            slotText.text = slotCount.ToString();
            speedText.text = $"{speed:0.#}";
            upkeepText.text = loc.PerDay($"{upkeep:+0.#;-0.#;0}");

            slotDiffText.gameObject.SetActive(false);
            speedDiffText.gameObject.SetActive(false);
            upkeepDiffText.gameObject.SetActive(false);
        }

        public void SetDiffs(int slotDiff, float speedDiff, float upkeepDiff)
        {
            slotDiffText.text = $"{slotDiff:+0;-0;0}".WithStyle(Style.Good);
            speedDiffText.text = $"{speedDiff:+0.#;-0.#;0}".WithStyle(Style.Good);
            upkeepDiffText.text = $"{upkeepDiff:+0.#;-0.#;0}".WithStyle(Style.Bad);

            slotDiffText.gameObject.SetActive(true);
            speedDiffText.gameObject.SetActive(true);
            upkeepDiffText.gameObject.SetActive(true);
        }
    }
}