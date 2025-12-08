using Common;
using Common.Modifiable;
using Features.Towns;
using Features.Towns.Reputation.Config;

namespace Features.Trade.Logic.Price
{
    /// <summary>
    /// The players reputation with a town influences the price of all trades.
    /// </summary>
    public sealed class ReputationPriceModifier : BasePercentageModifier
    {
        private readonly TradeType _tradeType;
        private readonly ReputationConfig reputationConfig = ConfigurationManager.Configurations.ReputationConfig;
        private readonly string _townName;

        public ReputationPriceModifier(Town town, TradeType tradeType) : base(0f, string.Empty)
        {
            _tradeType = tradeType;
            _townName = town.Name;

            town.ReputationManager.Reputation.Observe(OnReputationChange);
        }

        private void OnReputationChange(float reputation)
        {
            Description.Value = GetDescription(reputation);
            Value.Value = GetValue(reputation);
        }

        private string GetDescription(float reputation)
        {
            var townLikesPlayer = reputation >= 0;
            var likesOrDislikes = townLikesPlayer ? "likes" : "dislikes";
            var repPerPricePercent = reputationConfig.ReputationPerPricePercent;
            return
                $"{_townName} {likesOrDislikes} you! Your reputation: {(int)reputation} (1% per {repPerPricePercent} reputation points)";
        }

        private float GetValue(float reputation)
        {
            // buying from a town, with positive rep should lower prices
            var sign = _tradeType == TradeType.Buy ? -1 : 1;
            var repPerPricePercent = reputationConfig.ReputationPerPricePercent;
            return reputation * 0.01f * sign / repPerPricePercent;
        }
    }
}