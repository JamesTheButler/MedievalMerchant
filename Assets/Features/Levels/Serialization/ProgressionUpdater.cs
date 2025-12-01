using Infrastructure;
using UnityEngine;

namespace Features.Levels.Serialization
{
    public sealed class ProgressionUpdater : MonoBehaviour
    {
        public void LevelCompleted()
        {
            var completionDate = GameplayContext.Model.Date;
            var levelSaveData = new CompletedLevelSaveData(completionDate);
            var levelIndex = GlobalContext.CurrentLevelInfo!.InternalIndex;

            GlobalContext.ProgressModel.UpdateCompletedLevel(levelIndex, levelSaveData);
        }
    }
}