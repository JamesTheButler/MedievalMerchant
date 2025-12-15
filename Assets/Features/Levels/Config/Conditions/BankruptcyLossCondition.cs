using Common;
using Common.Types;
using Features.Player;
using Infrastructure;
using UnityEngine;

namespace Features.Levels.Config.Conditions
{
    [CreateAssetMenu(
        fileName = nameof(BankruptcyLossCondition),
        menuName = AssetMenu.ConditionsFolder + nameof(BankruptcyLossCondition))]
    public sealed class BankruptcyLossCondition : LossCondition
    {
        [SerializeField]
        private int maxBankruptcyDurationInDays = 7;

        [SerializeField]
        private int bankruptcyFundsThreshold;

        public override string CompletionMessage => "You've run out of money!";
        private Date _currentDate, _bankruptcyDate;
        private PlayerModel _playerModel;

        private bool _isBankrupt;

        public override ConditionType Type => ConditionType.BankruptcyLossCondition;
        public override string Description => GetDescription();

        public override void Initialize()
        {
            _isBankrupt = false;
            _currentDate = GameplayContext.Instance.Model.Date;
            _playerModel = GameplayContext.Instance.Model.Player;

            _playerModel.Inventory.Funds.Observe(OnPlayerFundsChanged, false);

            Progress = new Progress(maxBankruptcyDurationInDays, FormatProgress);
        }

        private void OnPlayerFundsChanged(float funds)
        {
            // entered bankruptcy countdown
            if (!_isBankrupt && funds < bankruptcyFundsThreshold)
            {
                _isBankrupt = true;
                _bankruptcyDate = _currentDate + maxBankruptcyDurationInDays;
                _currentDate.Changed += OnGameDateChanged;
                Progress.SetProgress(0);
            }

            // left bankruptcy countdown
            else if (_isBankrupt && funds > bankruptcyFundsThreshold)
            {
                _isBankrupt = false;
                _bankruptcyDate = null;
                _currentDate.Changed -= OnGameDateChanged;
                Progress.SetProgress(0);
            }
        }

        private void OnGameDateChanged(Date currentDate)
        {
            if (!_isBankrupt)
            {
                Progress.SetProgress(0);
                return;
            }

            var diff = DateExtensions.DiffInDays(_bankruptcyDate, currentDate);
            if (diff <= 0)
            {
                Progress.Complete();
                return;
            }

            if (diff >= maxBankruptcyDurationInDays)
            {
                Progress.SetProgress(0);
                return;
            }

            Progress.SetProgress(maxBankruptcyDurationInDays - diff);
        }


        private static string FormatProgress(int currentValue, int maxValue)
        {
            return $"{maxValue - currentValue} days left in bankruptcy";
        }

        private string GetDescription()
        {
            return
                $"You lose if you have less than {bankruptcyFundsThreshold} coin for more than {maxBankruptcyDurationInDays} days.";
        }
    }
}