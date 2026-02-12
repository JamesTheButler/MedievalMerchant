using Common.Infrastructure.Modifiable;
using Common.UI.Art;
using Common.UI.Tooltips;
using Common.UI.Utility;
using Common.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Player.UI
{
    public sealed class PlayerMiniUI : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text fundsText, fundsChangeText;

        [SerializeField, Required]
        private ModifiableTooltipHandler modifiableTooltip;

        [SerializeField, Required]
        private SimpleAnimationHandler simpleAnimationHandler;

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

            var is0 = fundsChange.IsApproximately(0f);
            fundsChangeText.gameObject.SetActive(!is0);

            Style style;
            if (is0)
            {
                style = Style.Default;
            }
            else if (fundsChange > 0)
            {
                style = Style.Good;
            }
            else
            {
                style = Style.Bad;
            }

            fundsChangeText.text = formattedText.WithStyle(style);
        }

        public void PlayCoinEffect()
        {
            Debug.LogError("Playing Animation");
            simpleAnimationHandler.Play();
        }
    }
}