using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuySellPopup : MonoBehaviour
    {
        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Button sellButton;

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