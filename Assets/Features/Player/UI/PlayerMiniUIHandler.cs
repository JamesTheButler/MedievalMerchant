using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using Features.Trade;
using Features.Trade.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Player.UI
{
    public sealed class PlayerMiniUIHandler : InitializableBehavior
    {
        [SerializeField, Required]
        private PlayerMiniUI playerMiniUI;

        private readonly Bindings _bindings = new();

        public override void Initialize()
        {
            var model = GameplayContext.Instance.Model;
            var player = model.Player;
            var tradeService = GameplayContext.Instance.Services.TradeService;

            playerMiniUI.SetFundsChangeTooltipTarget(player.FundsChange);
            _bindings.Track(
                player.Inventory.Funds.Observe(OnFundsChanged),
                player.FundsChange.Observe(OnFundsChangeChanged),
                tradeService.TradeCompleted.Observe(OnTradeCompleted)
            );
        }

        private void OnTradeCompleted(CompletedTrade trade)
        {
            if (trade.TradeType != TradeType.Sell) return;

            playerMiniUI.PlayCoinEffect();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _bindings.Unbind();
        }

        private void OnFundsChangeChanged(float fundsChange)
        {
            playerMiniUI.SetFundsChange(fundsChange);
        }

        private void OnFundsChanged(float funds)
        {
            playerMiniUI.SetFunds(funds);
        }
    }
}