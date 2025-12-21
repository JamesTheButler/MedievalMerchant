namespace Features.Player.Logic
{
    public record TradeTrackInfo(int Amount, float TotalPrice)
    {
        public float AveragePrice => TotalPrice / Amount;
    }
}