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
                FundsWinConditionData fundsWinConditionData =>
                    new FundsWinCondition(fundsWinConditionData),

                BankruptcyLossConditionData bankruptcyLossConditionData =>
                    new BankruptcyLossCondition(bankruptcyLossConditionData),

                TimeoutLossConditionData timeoutLossConditionData =>
                    new TimeoutLossCondition(timeoutLossConditionData),

                TownTierWinConditionData townTierWinConditionData =>
                    new TownTierWinCondition(townTierWinConditionData),

                _ => throw new ArgumentOutOfRangeException(nameof(conditionData))
            };
        }
    }
}