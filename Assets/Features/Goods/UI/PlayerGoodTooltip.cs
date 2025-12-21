using System.Collections.Generic;
using Common.Infrastructure;
using Common.Types;
using Features.Player.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Goods.UI
{
    public sealed class PlayerGoodTooltip : GoodTooltip
    {
        [SerializeField, Required]
        private TMP_Text averagePurchasePriceText;

        private TradeTracker _tradeTracker;

        protected override void Awake()
        {
            base.Awake();
            _tradeTracker = GameplayContext.Instance.Model.Player.TradeTracker;
        }

        protected override void UpdateUI(Good data)
        {
            base.UpdateUI(data);

            var purchasedAverage = _tradeTracker.TrackedGoods.GetValueOrDefault(data)?.AveragePrice ?? 0f;
            averagePurchasePriceText.text = $"{purchasedAverage:0.##}";
        }

        public override void Reset() { }
    }
}