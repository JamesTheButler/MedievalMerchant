using Common;
using UnityEngine;

namespace Features.Levels.Serialization
{
    public sealed class ProgressionUpdater : MonoBehaviour
    {
        private Model _model;
        
        private void Awake()
        {
            _model = Model.Instance;
        }

        public void LevelCompleted()
        {
            var completionDate = Model.Instance.Date;
            var levelSaveData = new CompletedLevelSaveData(completionDate);
            var levelIndex = _model.LevelInfo.InternalIndex;
            
            GlobalContext.ProgressModel.UpdateCompletedLevel(levelIndex, levelSaveData);
        }
    }
}