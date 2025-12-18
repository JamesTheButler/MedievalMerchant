using Common.Infrastructure.Modifiable;
using Common.UI.Tooltips;
using Common.UI.Utility;
using Common.Utility;
using TMPro;
using UnityEngine;

namespace Features.Player.UI
{
    public sealed class PlayerMiniUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text fundsText, fundsChangeText;

        [SerializeField]
        private ModifiableTooltipHandler modifiableTooltip;

        private ModifiableVariable _fundsChange;

        public void SetFunds(float funds)
        {
            fundsText.text = funds.ToString("0.#");
        }

        public void SetFundsChangeTooltipTarget(ModifiableVariable fundsChange)
        {
            modifiableTooltip.SetData(fundsChange);
        }

        public void SetFundsChange(float fundsChange)
        {
            var formattedText = $"{fundsChange.Sign()}{fundsChange:0.#}";

            if (fundsChange.IsApproximately(0f))
            {
                fundsChangeText.text = formattedText.WithDefaultStyle();
            }
            else if (fundsChange < 0)
            {
                fundsChangeText.text = formattedText.WithBadStyle();
            }
            else if (fundsChange > 0)
            {
                fundsChangeText.text = formattedText.WithGoodStyle();
            }
        }
    }
}