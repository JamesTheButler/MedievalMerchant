using Common.Types;
using Features.Towns;
using Features.Trade;
using Features.Trade.UI;
using Infrastructure;
using NaughtyAttributes;
using UnityEngine;

namespace UI
{
    public sealed class TradeUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private TradeUI tradeUI;

        private void Start()
        {
            tradeUI.gameObject.SetActive(false);

            GameplayContext.Selection.TownSelected += OnSelectedTownChanged;
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
            tradeUI.Hide();
        }
    }
}