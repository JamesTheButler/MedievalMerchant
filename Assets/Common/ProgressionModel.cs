using System;
using System.Collections.Generic;
using Features.Levels.Serialization;
using UnityEngine;

namespace Common
{
    public sealed class ProgressionModel : MonoBehaviour
    {
        private const int LevelCount = 5;

        public static ProgressionModel Instance;

        public Action<int> OngoingLevelStatusChanged;
        public Action<int> CompletedLevelStatusChanged;

        public IReadOnlyList<OngoingLevelSaveData> OngoingLevels => _ongoingLevels;
        public IReadOnlyList<CompletedLevelSaveData> CompletedLevels => _completedLevels;

        private IGamePersistenceService _gamePersistenceService;

        private readonly List<OngoingLevelSaveData> _ongoingLevels = new(new OngoingLevelSaveData[5]);
        private readonly List<CompletedLevelSaveData> _completedLevels = new(new CompletedLevelSaveData[5]);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            _gamePersistenceService = new GamePersistenceService();
            LoadSaveGame();
        }

        private void LoadSaveGame()
        {
            for (var i = 0; i < LevelCount; i++)
            {
                _completedLevels[i] = _gamePersistenceService.GetCompletedLevelData(i);
                _ongoingLevels[i] = _gamePersistenceService.GetOngoingLevelData(i);
            }
        }

        public void UpdateOngoingLevel(int levelId, OngoingLevelSaveData saveData)
        {
            _ongoingLevels[levelId] = saveData;
            _gamePersistenceService.SaveOngoingLevel(levelId, saveData);
            OngoingLevelStatusChanged?.Invoke(levelId);
        }

        public void UpdateCompletedLevel(int levelId, CompletedLevelSaveData saveData)
        {
            _completedLevels[levelId] = saveData;
            _gamePersistenceService.SaveCompletedLevel(levelId, saveData);
            CompletedLevelStatusChanged?.Invoke(levelId);
        }

        public void ResetOngoingLevel(int levelId)
        {
            _ongoingLevels[levelId] = null;
            _gamePersistenceService.ResetOngoingLevel(levelId);
            OngoingLevelStatusChanged?.Invoke(levelId);
        }

        public void ResetCompletedLevel(int levelId)
        {
            _completedLevels[levelId] = null;
            _gamePersistenceService.ResetCompletedLevel(levelId);
            CompletedLevelStatusChanged?.Invoke(levelId);
        }

        public void Reset()
        {
            for (var i = 0; i < LevelCount; i++)
            {
                if (_completedLevels[i] != null)
                {
                    _completedLevels[i] = null;
                    CompletedLevelStatusChanged?.Invoke(i);
                }

                if (_ongoingLevels[i] != null)
                {
                    _ongoingLevels[i] = null;
                    OngoingLevelStatusChanged?.Invoke(i);
                }
            }

            _gamePersistenceService.ResetAllSaveData();
        }
    }
}