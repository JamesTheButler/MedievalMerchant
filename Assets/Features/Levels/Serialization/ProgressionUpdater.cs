using Common.Infrastructure;
using Common.Types;
using Features.Levels.Conditions.Model;
using UnityEngine;

namespace Features.Levels.Serialization
{
    public sealed class ProgressionSystem : ISystem
    {
        private LevelConditions _conditions;
        private Date _gameDate;
        private ProgressModel _progressModel;

        public void Initialize()
        {
            _conditions = GameplayContext.Instance.Model.Conditions;
            _gameDate = GameplayContext.Instance.Model.Date;
            _progressModel = GlobalContext.Instance.ProgressModel;

            _conditions.LevelWon += LevelCompleted;
        }

        public void CleanUp()
        {
            _conditions.LevelWon -= LevelCompleted;
        }

        private void LevelCompleted()
        {
            var levelIndex = GlobalContext.CurrentLevelInfo!.InternalIndex;

            var previousFinishDate = _progressModel.CompletedLevels[levelIndex]?.CompletionDate;
            var levelSaveData = new CompletedLevelSaveData(_gameDate);
            if (previousFinishDate == null || _gameDate < previousFinishDate)
            {
                _progressModel.UpdateCompletedLevel(levelIndex, levelSaveData);
            }
        }
    }
}