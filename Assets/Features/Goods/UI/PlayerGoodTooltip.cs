using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Player.Logic;
using Features.Towns;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Goods.UI
{
    public sealed class PlayerGoodTooltip : GoodTooltip
    {
        [SerializeField, Required]
        private GameObject currentPriceGroup;

        [SerializeField, Required]
        private TMP_Text averagePurchasePriceText;

        private TradeTracker _tradeTracker;
        private Selection _selection;

        protected override void Awake()
        {
            base.Awake();
            _tradeTracker = GameplayContext.Instance.Model.Player.TradeTracker;
            _selection = GameplayContext.Instance.Selection;
        }

        protected override void UpdateUI(Good data)
        {
            base.UpdateUI(data);

            var purchasedAverage = _tradeTracker.TrackedGoods.GetValueOrDefault(data)?.AveragePrice ?? 0f;
            averagePurchasePriceText.text = $"{purchasedAverage:0.##}";

            _selection.SelectedTown.Observe(OnSelectedTownChanged);
        }

        public override void Reset()
        {
            _selection.SelectedTown.StopObserving(OnSelectedTownChanged);
        }

        private void OnSelectedTownChanged(Town town)
        {
            var isTownSelected = town != null;
            currentPriceGroup.SetActive(isTownSelected);

            if (town == null)
                return;

            currentPriceLabel.text = $"{town.Name} {currentPriceLabel.text}";
        }
    }
}