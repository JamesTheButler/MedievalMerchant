using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Global;
using Common.Types;
using Features.Levels.Conditions.Model;

namespace Features.Levels.Serialization
{
    public sealed class ProgressionSystem : ISystem
    {
        private LevelConditions _conditions;
        private DateModel _gameDateModel;
        private ProgressModel _progressModel;

        public void Initialize()
        {
            _conditions = GameplayContext.Instance.Model.Conditions;
            _gameDateModel = GameplayContext.Instance.Model.DateModel;
            _progressModel = GlobalContext.Instance.Model.ProgressModel;

            _conditions.LevelWon.Observe(LevelCompleted);
        }

        public void CleanUp()
        {
            _conditions.LevelWon.StopObserving(LevelCompleted);
        }

        private void LevelCompleted()
        {
            var levelIndex = GlobalContext.CurrentLevelInfo!.InternalIndex;
            if (levelIndex < 0)
                return;

            var previousFinishDate = _progressModel.CompletedLevels[levelIndex]?.CompletionDate;
            var levelSaveData = new CompletedLevelSaveData(_gameDateModel.GameDate.Value);
            if (previousFinishDate == null || _gameDateModel.GameDate.Value < previousFinishDate)
            {
                _progressModel.UpdateCompletedLevel(levelIndex, levelSaveData);
            }
        }
    }
}