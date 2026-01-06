#nullable enable

using System;
using System.IO;
using Common.Infrastructure;
using UnityEngine;

namespace Features.Levels.Serialization
{
    public sealed class GamePersistenceService : IGamePersistenceService
    {
        private static readonly string SaveGameRootPath = Application.persistentDataPath;
        private static readonly string OngoingLevelPath = Path.Combine(SaveGameRootPath, "OngoingLevels");
        private static readonly string CompletedLevelPath = Path.Combine(SaveGameRootPath, "CompletedLevels");
        private static readonly string OngoingLevelFilenameTemplate = Path.Combine(OngoingLevelPath, "Level{0}.txt");

        private static readonly string CompletedLevelFilenameTemplate =
            Path.Combine(CompletedLevelPath, "Level{0}.txt");

        private readonly Lazy<ISerializer> _serializer = new(() => GlobalContext.Instance.Services.Serializer);

        public GamePersistenceService()
        {
            EnsureFoldersExist();
        }

        public CompletedLevelSaveData? GetCompletedLevelData(int levelId)
        {
            var filePath = string.Format(CompletedLevelFilenameTemplate, levelId);
            if (!File.Exists(filePath))
                return null;

            var fileContent = File.ReadAllText(filePath);
            return _serializer.Value.Deserialize<CompletedLevelSaveData>(fileContent);
        }

        public OngoingLevelSaveData? GetOngoingLevelData(int levelId)
        {
            var filePath = string.Format(OngoingLevelFilenameTemplate, levelId);
            if (!File.Exists(filePath))
                return null;

            var fileContent = File.ReadAllText(filePath);
            return _serializer.Value.Deserialize<OngoingLevelSaveData>(fileContent);
        }

        public void SaveCompletedLevel(int levelId, CompletedLevelSaveData saveData)
        {
            EnsureFoldersExist();
            var serializedSaveData = _serializer.Value.Serialize(saveData);
            var filePath = string.Format(CompletedLevelFilenameTemplate, levelId);
            File.WriteAllText(filePath, serializedSaveData);
        }

        public void ResetCompletedLevel(int levelId)
        {
            var filePath = string.Format(CompletedLevelFilenameTemplate, levelId);
            if (!File.Exists(filePath))
                return;

            File.Delete(filePath);
        }

        public void SaveOngoingLevel(int levelId, OngoingLevelSaveData saveData)
        {
            EnsureFoldersExist();
            var serializedSaveData = _serializer.Value.Serialize(saveData);
            var filePath = string.Format(OngoingLevelFilenameTemplate, levelId);
            File.WriteAllText(filePath, serializedSaveData);
        }

        public void ResetOngoingLevel(int levelId)
        {
            var filePath = string.Format(OngoingLevelFilenameTemplate, levelId);

            if (!File.Exists(filePath))
                return;

            File.Delete(filePath);
        }

        public void ResetAllOngoingLevels()
        {
            if (!Directory.Exists(OngoingLevelPath))
                return;

            Directory.Delete(OngoingLevelPath, true);
        }

        public void ResetAllCompletedLevels()
        {
            if (!Directory.Exists(CompletedLevelPath))
                return;

            Directory.Delete(CompletedLevelPath, true);
        }

        public void ResetAllSaveData()
        {
            ResetAllOngoingLevels();
            ResetAllCompletedLevels();
        }

        private static void EnsureFoldersExist()
        {
            Directory.CreateDirectory(SaveGameRootPath);
            Directory.CreateDirectory(OngoingLevelPath);
            Directory.CreateDirectory(CompletedLevelPath);
        }
    }
}