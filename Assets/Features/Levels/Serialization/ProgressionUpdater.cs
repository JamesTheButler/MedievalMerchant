using Infrastructure;
using UnityEngine;

namespace Features.Levels.Serialization
{
    public sealed class ProgressionUpdater : MonoBehaviour
    {
        public void LevelCompleted()
        {
            var completionDate = GameplayContext.Instance.Model.Date;
            var levelSaveData = new CompletedLevelSaveData(completionDate);
            var levelIndex = GlobalContext.CurrentLevelInfo!.InternalIndex;

            var progressModel = GlobalContext.Instance.ProgressModel;
            var previousFinishDate = progressModel.CompletedLevels[levelIndex]?.CompletionDate;

            if (previousFinishDate == null || completionDate < previousFinishDate)
            {
                progressModel.UpdateCompletedLevel(levelIndex, levelSaveData);
            }
        }
    }
}