using System;
using Features.Levels.Conditions.Data;

namespace Features.Levels.Conditions.Model
{
    public sealed class ConditionModelFactory
    {
        public ICondition Get(ConditionData conditionData)
        {
            return conditionData switch
            {
                FundsWinConditionData data => new FundsWinCondition(data),
                BankruptcyLossConditionData data => new BankruptcyLossCondition(data),
                TimeoutLossConditionData data => new TimeoutLossCondition(data),
                TownTierWinConditionData data => new TownTierWinCondition(data),
                LocalReputationWinConditionData data => new LocalReputationWinCondition(data),
                GlobalReputationWinConditionData data => new GlobalReputationWinCondition(data),

                _ => throw new ArgumentOutOfRangeException(nameof(conditionData))
            };
        }
    }
}