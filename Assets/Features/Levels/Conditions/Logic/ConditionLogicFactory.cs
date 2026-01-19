using System;
using Features.Levels.Conditions.Model;

namespace Features.Levels.Conditions.Logic
{
    public sealed class ConditionLogicFactory
    {
        public IConditionLogic Get(ICondition condition)
        {
            return condition switch
            {
                FundsWinCondition con => new FundsWinConditionLogic(con),
                BankruptcyLossCondition con => new BankruptcyLossConditionLogic(con),
                TimeoutLossCondition con => new TimeoutLossConditionLogic(con),
                TownTierWinCondition con => new TownTierWinConditionLogic(con),
                LocalReputationWinCondition con => new LocalReputationWinConditionLogic(con),
                GlobalReputationWinCondition con => new GlobalReputationWinConditionLogic(con),
                _ => throw new ArgumentOutOfRangeException(nameof(condition))
            };
        }
    }
}