using JetBrains.Annotations;

namespace Data.Trade
{
    public readonly struct TradeResult
    {
        public bool Success { get; }

        [CanBeNull]
        public string Error { get; }

        public static TradeResult Succeeded() => new(true, null);
        public static TradeResult Failed(string error) => new(false, error);

        private TradeResult(bool success, string error)
        {
            Success = success;
            Error = error;
        }
    }
}