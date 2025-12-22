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

        private Selection _selection;

        private void Start()
        {
            tradeUI.gameObject.SetActive(false);

            _selection = GameplayContext.Instance.Selection;
            _selection.SelectedTown.Observe(OnSelectedTownChanged);
        }

        private void OnDestroy()
        {
            _selection.SelectedTown.StopObserving(OnSelectedTownChanged);
        }

        public void Show(Good good, TradeType tradeType)
        {
            tradeUI.Hide();
            tradeUI.gameObject.SetActive(true);
            tradeUI.Initialize(good, tradeType);
        }

        private void OnSelectedTownChanged(Town town)
        {
            Hide();
        }

        private void Hide()
        {
            tradeUI.Hide();
        }
    }
}