using Data;
using UnityEngine;

namespace UI
{
    public class TradeUIHandler : MonoBehaviour
    {
        [SerializeField]
        private TradeUI tradeUI;

        private void Start()
        {
            tradeUI.gameObject.SetActive(false);

            SelectionModel.Instance.TownSelected += OnSelectedTownChanged;
        }

        private void OnSelectedTownChanged(Town town)
        {
            Hide();
        }

        public void Show(Good good, TradeType tradeType)
        {
            tradeUI.gameObject.SetActive(true);
            tradeUI.Initialize(good, tradeType);
        }

        public void Hide()
        {
            tradeUI.gameObject.SetActive(false);
            tradeUI.Reset();
        }
    }
}