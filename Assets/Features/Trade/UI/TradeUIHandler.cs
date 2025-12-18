using Common.Infrastructure;
using Common.Types;
using Features.Towns;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Trade.UI
{
    public sealed class TradeUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private TradeUI tradeUI;

        private void Start()
        {
            tradeUI.gameObject.SetActive(false);

            GameplayContext.Instance.Selection.TownSelected += OnSelectedTownChanged;
        }

        private void OnSelectedTownChanged(Town town)
        {
            Hide();
        }

        public void Show(Good good, TradeType tradeType)
        {
            tradeUI.Hide();
            tradeUI.gameObject.SetActive(true);
            tradeUI.Initialize(good, tradeType);
        }

        public void Hide()
        {
            tradeUI.Hide();
        }
    }
}