using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Common.UI.Elements.Cells
{
    public class InventoryCellBase : GoodCell
    {
        [SerializeField, Required]
        private TMP_Text amountText, amountTextSecondary;

        private int _amount;

        public void SetAmount(int amount)
        {
            if (_amount == amount)
                return;

            _amount = amount;

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