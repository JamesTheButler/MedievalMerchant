#nullable enable

namespace Features.Levels.Serialization
{
    public interface IGamePersistenceService
    {
        void SaveCompletedLevel(int levelId, CompletedLevelSaveData saveData);
        void ResetCompletedLevel(int levelId);
        void ResetAllCompletedLevels();
        CompletedLevelSaveData? GetCompletedLevelData(int levelId);

        void SaveOngoingLevel(int levelId, OngoingLevelSaveData saveData);
        void ResetOngoingLevel(int levelId);
        void ResetAllOngoingLevels();
        OngoingLevelSaveData? GetOngoingLevelData(int levelId);

        void ResetAllSaveData();
    }
}