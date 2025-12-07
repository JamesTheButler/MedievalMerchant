using Common.Types;

namespace Features.Towns.Reputation.Logic
{
    public sealed record ReputationLogEntry(Date Date, float RepChange, float CurrentRep, string Description);
}