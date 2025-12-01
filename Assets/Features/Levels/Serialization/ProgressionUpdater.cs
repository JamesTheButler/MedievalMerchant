using Common;
using Infrastructure;
using UnityEngine;

namespace Features.Levels.Serialization
{
    public sealed class ProgressionUpdater : MonoBehaviour
    {
        private GameplayModel _model;
        
        private void Awake()
        {
            _model = GameplayModel.Instance;
        }

        public void LevelCompleted()
        {
            var completionDate = GameplayModel.Instance.Date;
            var levelSaveData = new CompletedLevelSaveData(completionDate);
            var levelIndex = _model.LevelInfo.InternalIndex;
            
            GlobalContext.ProgressModel.UpdateCompletedLevel(levelIndex, levelSaveData);
        }
    }
}