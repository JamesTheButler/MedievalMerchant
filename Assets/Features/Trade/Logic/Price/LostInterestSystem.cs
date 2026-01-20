using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Goods.Selector;
using Features.Towns;

namespace Features.Trade.Logic.Price
{
    public sealed class LostInterestSystem : ISystem
    {
        private readonly Town _town;

        private TradeService _tradeService;
        private Date _gameDate;
        private Dictionary<Good, LostInterestPriceModifier> _modifier = new();

        public LostInterestSystem(Town town)
        {
            _town = town;
        }

        public void Initialize()
        {
            _tradeService = GameplayContext.Instance.Services.TradeService;
            _gameDate = GameplayContext.Instance.Model.Date;

            _tradeService.TradeCompleted.Observe(OnTradeCompleted);
        }

        public void CleanUp() { }

        private void OnTradeCompleted(TradeInfo tradeInfo)
        {
            if (tradeInfo.Town != _town)
                return;
        }

        private void AddModifier(Good good)
        {
            _town.PriceManager.AddModifier(_modifier[good], new SingleGoodSelector(good), TradeType.Sell);
        }
    }
}