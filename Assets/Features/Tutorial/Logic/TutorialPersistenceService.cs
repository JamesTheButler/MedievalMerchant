using System.Collections.Generic;
using System.IO;
using Common.Infrastructure;
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

        public IEnumerable<TutorialTopic> ReadCompletedTopics()
        {
            if (!File.Exists(CompletedTopicsFilePath))
                return new List<TutorialTopic>();

            var completedTopicsFile = File.ReadAllText(CompletedTopicsFilePath);
            return _serializer.Deserialize<IEnumerable<TutorialTopic>>(completedTopicsFile);
        }

        public void WriteCompletedTopics(IEnumerable<TutorialTopic> topics)
        {
            var serializedDict = _serializer.Serialize(topics);
            File.WriteAllText(CompletedTopicsFilePath, serializedDict);
        }
    }
}