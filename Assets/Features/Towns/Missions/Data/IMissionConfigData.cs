using Features.Towns.Missions.Results;

namespace Features.Towns.Missions.Data
{
    public interface IMissionConfigData
    {
        int LengthInDays { get; }
        int Volume { get; }
        IMissionResult GetReward();
        IMissionResult GetPenalty();
    }
}