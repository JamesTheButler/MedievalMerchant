namespace Features.Towns.Missions.Results
{
    public record UpgradeMissionPenalty(float ReputationPenalty, float GrowthPenalty) : IMissionResult;
}