using Common.Types;

namespace Features.Towns.Reputation.Logic
{
    public sealed record ReputationLogEntry(DateModel DateModel, float RepChange, float CurrentRep, string Description);
}