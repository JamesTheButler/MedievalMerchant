using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Towns;
using Features.Towns.Reputation.Data;

namespace Features.Trade.Logic.Price
{
    /// <summary>
    /// The players reputation with a town influences the price of all trades.
    /// </summary>
    public sealed class ReputationPriceModifier : BasePercentageModifier
    {
        private readonly TradeType _tradeType;
        private readonly ReputationConfig _reputationConfig;
        private readonly string _townName;

        public ReputationPriceModifier(Town town, TradeType tradeType) : base(0f, string.Empty)
        {
            _reputationConfig = ConfigurationManager.Configurations.ReputationConfig;

            _tradeType = tradeType;
            _townName = town.Name;
            town.ReputationModel.Reputation.Observe(Update);
        }

        private void Update(float reputation)
        {
            Description.Value = GetDescription(reputation);
            Value.Value = GetValue(reputation);
        }

        private string GetDescription(float reputation)
        {
            var townLikesPlayer = reputation >= 0;
            var likesOrDislikes = townLikesPlayer ? "likes" : "dislikes";
            return $"{_townName} {likesOrDislikes} you! Your reputation: {reputation:0.#}";
        }

        private float GetValue(float reputation)
        {
            // buying from a town, with positive rep should lower prices
            var sign = _tradeType == TradeType.Buy ? -1 : 1;
            var repPerPricePercent = _reputationConfig.ReputationPerPricePercent;
            return reputation * 0.01f * sign / repPerPricePercent;
        }
    }
}