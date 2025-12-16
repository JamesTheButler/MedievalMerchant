using System;
using Features.Levels.Conditions.Model;

namespace Features.Levels.Conditions.Logic
{
    public sealed class ConditionLogicFactory
    {
        public IConditionLogic Get(Model.ICondition condition)
        {
            return condition switch
            {
                FundsWinCondition fundsWinCondition =>
                    new FundsWinConditionLogic(fundsWinCondition),

                BankruptcyLossCondition bankruptcyLossCondition =>
                    new BankruptcyLossConditionLogic(bankruptcyLossCondition),

                TimeoutLossCondition timeoutLossCondition =>
                    new TimeoutLossConditionLogic(timeoutLossCondition),

                TownTierWinCondition townTierWinCondition =>
                    new TownTierWinConditionLogic(townTierWinCondition),
                _ => throw new ArgumentOutOfRangeException(nameof(condition))
            };
        }
    }
}