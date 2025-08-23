using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuySellPopup : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text goodNameText;

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Button sellButton;

        public void SetGood(Good good)
        {
            goodNameText.text = Setup.Instance.GoodInfoManager.GoodInfos[good].GoodName;
        }

        public void CanBuy(bool canBuy)
        {
            buyButton.interactable = canBuy;
        }

        public void CanSell(bool canSell)
        {
            sellButton.interactable = canSell;
        }
    }
}