using Common;
using UnityEngine;

namespace Features.Levels.Conditions.Data
{
    [CreateAssetMenu(
        fileName = nameof(BankruptcyLossConditionData),
        menuName = AssetMenu.ConditionsFolder + nameof(BankruptcyLossConditionData))]
    public sealed class BankruptcyLossConditionData : LossConditionData
    {
        [field: SerializeField]
        public int MaxBankruptcyDurationInDays { get; private set; } = 7;

        [field: SerializeField]
        public int BankruptcyFundsThreshold { get; private set; }

        public override ConditionType Type => ConditionType.BankruptcyLossCondition;

        public override string Description =>
            $"You lose if you have less than {BankruptcyFundsThreshold} coin for more than {MaxBankruptcyDurationInDays} days.";
    }
}