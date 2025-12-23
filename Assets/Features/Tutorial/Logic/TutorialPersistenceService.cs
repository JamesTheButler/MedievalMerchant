using System.Collections.Generic;
using System.IO;
using Common.Infrastructure;
using Common.Utility;
using UnityEngine;

namespace Features.Tutorial.Logic
{
    public sealed class TutorialPersistenceService : IService
    {
        private static readonly string SaveGameRootPath = Application.persistentDataPath;
        private static readonly string CompletedTopicsFilePath = Path.Combine(SaveGameRootPath, "tutorial.txt");

        private readonly ISerializer _serializer;

        public TutorialPersistenceService(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public void Initialize() { }
        public void CleanUp() { }

        public IDictionary<TutorialTopic, bool> ReadCompletedTopics()
        {
            if (!File.Exists(CompletedTopicsFilePath))
                return EnumExtensions.MakeDictionary<TutorialTopic, bool>(false);

            var completedTopicsFile = File.ReadAllText(CompletedTopicsFilePath);
            return _serializer.Deserialize<Dictionary<TutorialTopic, bool>>(completedTopicsFile);
        }

        public void WriteCompletedTopics(IDictionary<TutorialTopic, bool> topics)
        {
            var serializedDict = _serializer.Serialize(topics.ToDictionary());
            File.WriteAllText(CompletedTopicsFilePath, serializedDict);
        }
    }
}