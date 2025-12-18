using System.Collections.Generic;
using Common.Infrastructure;
using Features.Levels.Conditions.Model;

namespace Features.Levels.Conditions.Logic
{
    /// <summary>
    /// Manages condition logics.
    /// </summary>
    public sealed class ConditionSystem : ISystem
    {
        private LevelConditions _model;

        private readonly ConditionLogicFactory _logicFactory = new();
        private readonly List<IConditionLogic> _conditionLogics = new();

        public void Initialize()
        {
            _model = GameplayContext.Instance.Model.Conditions;
            foreach (var condition in _model.WinConditions)
            {
                var logic = _logicFactory.Get(condition);
                logic.Initialize();
                _conditionLogics.Add(logic);
            }
            
            foreach (var condition in _model.LossConditions)
            {
                var logic = _logicFactory.Get(condition);
                logic.Initialize();
                _conditionLogics.Add(logic);
            }
        }

        public void CleanUp()
        {
            foreach (var conditionLogic in _conditionLogics)
            {
                conditionLogic.CleanUp();
            }
        }
    }
}