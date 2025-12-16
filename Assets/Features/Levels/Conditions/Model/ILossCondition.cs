namespace Features.Levels.Conditions.Model
{
    public interface ILossCondition : ICondition
    {
        string GameOverMessage { get; }
    }
}