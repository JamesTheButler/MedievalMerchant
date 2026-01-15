using System;
using System.Collections.Generic;
using Features.Levels.Serialization;

namespace Common.Infrastructure.Global
{
    public sealed class ProgressModel
    {
        private const int LevelCount = 5;

        public Action<int> OngoingLevelStatusChanged;
        public Action<int> CompletedLevelStatusChanged;

        public IReadOnlyList<OngoingLevelSaveData> OngoingLevels => _ongoingLevels;
        public IReadOnlyList<CompletedLevelSaveData> CompletedLevels => _completedLevels;
        private  GamePersistenceService _persistenceService;

        private readonly List<OngoingLevelSaveData> _ongoingLevels = new(new OngoingLevelSaveData[5]);
        private readonly List<CompletedLevelSaveData> _completedLevels = new(new CompletedLevelSaveData[5]);

        public void Initialize()
        {
            _persistenceService = GlobalContext.Instance.PersistenceServices.GamePersistenceService;
            LoadSaveGame();
        }

        private void LoadSaveGame()
        {
            for (var i = 0; i < LevelCount; i++)
            {
                _completedLevels[i] = _persistenceService.GetCompletedLevelData(i);
                _ongoingLevels[i] = _persistenceService.GetOngoingLevelData(i);
            }
        }

        public void UpdateOngoingLevel(int levelId, OngoingLevelSaveData saveData)
        {
            _ongoingLevels[levelId] = saveData;
            _persistenceService.SaveOngoingLevel(levelId, saveData);
            OngoingLevelStatusChanged?.Invoke(levelId);
        }

        public void UpdateCompletedLevel(int levelId, CompletedLevelSaveData saveData)
        {
            _completedLevels[levelId] = saveData;
            _persistenceService.SaveCompletedLevel(levelId, saveData);
            CompletedLevelStatusChanged?.Invoke(levelId);
        }

        public void ResetOngoingLevel(int levelId)
        {
            _ongoingLevels[levelId] = null;
            _persistenceService.ResetOngoingLevel(levelId);
            OngoingLevelStatusChanged?.Invoke(levelId);
        }

        public void ResetCompletedLevel(int levelId)
        {
            _completedLevels[levelId] = null;
            _persistenceService.ResetCompletedLevel(levelId);
            CompletedLevelStatusChanged?.Invoke(levelId);
        }

        public void ResetCompletedLevels()
        {
            for (var i = 0; i < LevelCount; i++)
            {
                if (_completedLevels[i] != null)
                {
                    _completedLevels[i] = null;
                    CompletedLevelStatusChanged?.Invoke(i);
                }
            }

            _persistenceService.ResetAllCompletedLevels();
        }

        public void ResetOngoingLevels()
        {
            for (var i = 0; i < LevelCount; i++)
            {
                if (_ongoingLevels[i] != null)
                {
                    _ongoingLevels[i] = null;
                    OngoingLevelStatusChanged?.Invoke(i);
                }
            }

            _persistenceService.ResetAllOngoingLevels();
        }

        public void Reset()
        {
            ResetCompletedLevels();
            ResetOngoingLevels();
            _persistenceService.ResetAllSaveData();
        }
    }
}