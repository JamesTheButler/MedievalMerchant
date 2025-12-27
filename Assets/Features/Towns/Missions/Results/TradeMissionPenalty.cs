namespace Features.Towns.Missions.Results
{
    public record TradeMissionPenalty(float ReputationPenalty, float GrowthPenalty) : IMissionResult;
}