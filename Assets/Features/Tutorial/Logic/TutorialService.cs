using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Utility;

namespace Features.Tutorial.Logic
{
    public sealed class TutorialService : IService
    {
        public event Action<TutorialTopic, bool> TopicCompletionChanged;
        public event Action<TutorialTopic> OpenTutorialRequest;

        public IReadOnlyDictionary<TutorialTopic, bool> CompletedChapters => _completedChapters;

        private readonly TutorialPersistenceService _persistenceService;
        private readonly Dictionary<TutorialTopic, bool> _completedChapters;

        public TutorialService(TutorialPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
            _completedChapters = EnumExtensions.MakeDictionary<TutorialTopic, bool>(false);
        }

        public void Initialize()
        {
            var persistedTopics = _persistenceService.ReadCompletedTopics();

            foreach (var topic in persistedTopics)
            {
                _completedChapters[topic] = true;
                TopicCompletionChanged?.Invoke(topic, true);
            }
        }

        public void CleanUp() { }

        public void ResetCompletedTopics()
        {
            foreach (var (topic, isCompleted) in _completedChapters.ToArray())
            {
                if (!isCompleted)
                    continue;

                _completedChapters[topic] = false;
                TopicCompletionChanged?.Invoke(topic, false);
            }

            Persist();
        }

        public void OpenTutorial(TutorialTopic topic)
        {
            OpenTutorialRequest?.Invoke(topic);

            CompleteTopic(topic);
        }

        private void CompleteTopic(TutorialTopic topic)
        {
            // skip if already completed
            if (_completedChapters[topic])
                return;

            _completedChapters[topic] = true;
            TopicCompletionChanged?.Invoke(topic, true);
            Persist();
        }

        private void Persist()
        {
            _persistenceService.WriteCompletedTopics(_completedChapters.Where(kv => kv.Value).Select(kv => kv.Key));
        }
    }
}