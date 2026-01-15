using System.IO;
using Common.Infrastructure;
using Common.Infrastructure.Global;
using Common.Infrastructure.Serialization;

namespace Features.Levels.Serialization
{
    public sealed class GamePersistenceService : IService
    {
        private static readonly string OngoingLevelPath = Path.Combine(PersistenceLocation.Levels, "OngoingLevels");
        private static readonly string CompletedLevelPath = Path.Combine(PersistenceLocation.Levels, "CompletedLevels");
        private static readonly string OngoingLevelFilenameTemplate = Path.Combine(OngoingLevelPath, "Level{0}.txt");

        private static readonly string CompletedLevelFilenameTemplate =
            Path.Combine(CompletedLevelPath, "Level{0}.txt");

        private ISerializer _serializer;

        public void Initialize()
        {
            _serializer = GlobalContext.Instance.PersistenceServices.Serializer;
            EnsureFoldersExist();
        }

        public void CleanUp() { }

        public CompletedLevelSaveData GetCompletedLevelData(int levelId)
        {
            var filePath = string.Format(CompletedLevelFilenameTemplate, levelId);
            if (!File.Exists(filePath))
                return null;

            var fileContent = File.ReadAllText(filePath);
            return _serializer?.Deserialize<CompletedLevelSaveData>(fileContent);
        }

        public OngoingLevelSaveData GetOngoingLevelData(int levelId)
        {
            var filePath = string.Format(OngoingLevelFilenameTemplate, levelId);
            if (!File.Exists(filePath))
                return null;

            var fileContent = File.ReadAllText(filePath);
            return _serializer.Deserialize<OngoingLevelSaveData>(fileContent);
        }

        public void SaveCompletedLevel(int levelId, CompletedLevelSaveData saveData)
        {
            EnsureFoldersExist();
            var serializedSaveData = _serializer.Serialize(saveData);
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
            var serializedSaveData = _serializer.Serialize(saveData);
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
            Directory.CreateDirectory(PersistenceLocation.Levels);
            Directory.CreateDirectory(OngoingLevelPath);
            Directory.CreateDirectory(CompletedLevelPath);
        }
    }
}