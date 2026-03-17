namespace Features.Towns.Missions.Results
{
    public record TradeMissionPenalty(float Reputation, float Growth) : IMissionResult;
}