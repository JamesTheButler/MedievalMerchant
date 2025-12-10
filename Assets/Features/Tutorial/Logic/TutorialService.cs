using System;
using System.Collections.Generic;
using Common;

namespace Features.Tutorial.Logic
{
    public sealed class TutorialService : IService
    {
        public event Action<TutorialTopic, bool> TopicCompletionChanged;
        public event Action<TutorialTopic> OpenTutorialRequest;

        public IReadOnlyDictionary<TutorialTopic, bool> CompletedChapters => _completedChapters;

        private readonly TutorialPersistenceService _persistenceService;
        private Dictionary<TutorialTopic, bool> _completedChapters = new();

        public TutorialService(TutorialPersistenceService persistenceService)
        {
            _persistenceService = persistenceService;
        }

        public void Initialize()
        {
            var persistedTopics = _persistenceService.ReadCompletedTopics();
            _completedChapters = new Dictionary<TutorialTopic, bool>(persistedTopics);
            foreach (var (chapter, isCompleted) in persistedTopics)
            {
                TopicCompletionChanged?.Invoke(chapter, isCompleted);
            }
        }

        public void CleanUp()
        {
            _completedChapters.Clear();
        }

        public void ResetCompletedTopics()
        {
            foreach (var (topic, isCompleted) in _completedChapters)
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
            _persistenceService.WriteCompletedTopics(_completedChapters);
        }
    }
}