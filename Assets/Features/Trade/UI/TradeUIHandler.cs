using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements;
using Features.Towns;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Trade.UI
{
    public sealed class TradeUIHandler : InitializableBehavior
    {
        [SerializeField, Required]
        private TradeUI tradeUI;

        private Selection _selection;

        public override void Initialize()
        {
            tradeUI.gameObject.SetActive(false);

            _selection = GameplayContext.Instance.Selection;
            _selection.SelectedTown.Observe(OnSelectedTownChanged);
        }

        public override void CleanUp()
        {
            _selection.SelectedTown.StopObserving(OnSelectedTownChanged);
        }

        public void Show(Good good, TradeType tradeType)
        {
            tradeUI.Close();
            tradeUI.SetUp(good, tradeType);
            tradeUI.Open();
        }

        private void OnSelectedTownChanged(Town town)
        {
            tradeUI.Close();
        }
    }
}