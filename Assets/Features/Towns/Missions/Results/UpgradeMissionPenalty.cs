namespace Features.Towns.Missions.Results
{
    public record UpgradeMissionPenalty(float Reputation, float Growth) : IMissionResult;
}