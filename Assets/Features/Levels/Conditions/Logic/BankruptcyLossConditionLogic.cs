using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Levels.Conditions.Model;
using Features.Player.Logic;

namespace Features.Levels.Conditions.Logic
{
    public sealed class BankruptcyLossConditionLogic : IConditionLogic
    {
        private readonly BankruptcyLossCondition _condition;

        private Date _currentDate, _bankruptcyDate;
        private PlayerModel _playerModel;
        private bool _isBankrupt;

        public BankruptcyLossConditionLogic(BankruptcyLossCondition condition)
        {
            _condition = condition;
        }

        public void Initialize()
        {
            _currentDate = GameplayContext.Instance.Model.Date;
            _playerModel = GameplayContext.Instance.Model.Player;

            _playerModel.Inventory.Funds.Observe(OnPlayerFundsChanged, false);
        }

        public void CleanUp()
        {
            _playerModel.Inventory.Funds.StopObserving(OnPlayerFundsChanged);
        }

        private void OnPlayerFundsChanged(float funds)
        {
            // entered bankruptcy countdown
            if (!_isBankrupt && funds < _condition.BankruptcyFundsThreshold)
            {
                _isBankrupt = true;
                _bankruptcyDate = _currentDate + _condition.MaxBankruptcyDurationInDays;
                _currentDate.Changed += OnGameDateChanged;
                _condition.Progress.SetProgress(0);
            }

            // left bankruptcy countdown
            else if (_isBankrupt && funds > _condition.BankruptcyFundsThreshold)
            {
                _isBankrupt = false;
                _bankruptcyDate = null;
                _currentDate.Changed -= OnGameDateChanged;
                _condition.Progress.SetProgress(0);
            }
        }

        private void OnGameDateChanged(Date currentDate)
        {
            if (!_isBankrupt)
            {
                _condition.Progress.SetProgress(0);
                return;
            }

            var diff = DateExtensions.DiffInDays(_bankruptcyDate, currentDate);
            if (diff <= 0)
            {
                _condition.Progress.Complete();
                return;
            }

            if (diff >= _condition.MaxBankruptcyDurationInDays)
            {
                _condition.Progress.SetProgress(0);
                return;
            }

            _condition.Progress.SetProgress(_condition.MaxBankruptcyDurationInDays - diff);
        }
    }
}