using Common.Infrastructure.Observation;

namespace Features.Levels.Conditions.Model
{
    public interface ILossCondition : ICondition
    {
        string GameOverMessage { get; }
        string WarningMessage { get; }
        Observable<bool> IsClose { get; }
    }
}