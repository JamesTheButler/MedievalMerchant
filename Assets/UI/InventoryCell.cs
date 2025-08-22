using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryCell : MonoBehaviour
    {
        [SerializeField]
        private Image goodIcon;

        [SerializeField]
        private Image disabledIcon;

        [SerializeField]
        private Image productionIcon;

        [SerializeField]
        private TMP_Text amountText;

        public void SetGood(Good good)
        {
            goodIcon.sprite = Setup.Instance.GoodInfoManager.GoodInfos[good].Icon;
        }

        public void SetAmount(int amount)
        {
            if (amount <= 0)
            {
                amountText.gameObject.SetActive(false);
                disabledIcon.gameObject.SetActive(true);
            }
            else
            {
                amountText.gameObject.SetActive(true);
                disabledIcon.gameObject.SetActive(false);
                amountText.text = amount.ToString();
            }
        }

        public void SetIsProduced(bool isProduced)
        {
            productionIcon.gameObject.SetActive(isProduced);
        }
    }
}