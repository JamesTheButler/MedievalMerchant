namespace Features.Towns.Missions.Results
{
    public record TradeMissionReward(float Coin, float Reputation, float Growth) : IMissionResult;
}