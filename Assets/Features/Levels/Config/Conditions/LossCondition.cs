namespace Features.Levels.Config.Conditions
{
    public abstract class LossCondition : Condition
    {
        public abstract string CompletionMessage { get; }
    }
}