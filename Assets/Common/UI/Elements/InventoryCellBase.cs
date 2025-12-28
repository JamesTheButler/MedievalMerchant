using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Common.UI.Elements
{
    public class InventoryCellBase : GoodCell
    {
        [SerializeField, Required]
        private TMP_Text amountText, amountTextSecondary;

        public void SetAmount(int amount)
        {
            if (amount <= 0)
            {
                amountText?.gameObject.SetActive(false);
            }
            else
            {
                amountText?.gameObject.SetActive(true);
                amountText?.SetText(amount.ToString());
                amountTextSecondary?.SetText(amount.ToString());
            }

            SetEnabled(amount > 0);
        }
    }
}