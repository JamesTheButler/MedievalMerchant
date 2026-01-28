using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Global;
using Common.Utility;

namespace Features.Tutorial.Logic
{
    public sealed class TutorialService : IService
    {
        public event Action<TutorialTopic, bool> TopicCompletionChanged;
        public event Action<TutorialTopic> OpenTutorialRequest;

        public bool HasCompletedIntro { get; private set; }
        public IReadOnlyDictionary<TutorialTopic, bool> CompletedChapters => _completedChapters;

        private readonly Dictionary<TutorialTopic, bool> _completedChapters =
            EnumExtensions.MakeDictionary<TutorialTopic, bool>(false);

        private TutorialPersistenceService _persistenceService;

        public void Initialize()
        {
            _persistenceService = GlobalContext.Instance.PersistenceServices.TutorialPersistenceService;
            var persistedTopics = _persistenceService.Load();

            foreach (var topic in persistedTopics.CompletedTopics)
            {
                _completedChapters[topic] = true;
                TopicCompletionChanged?.Invoke(topic, true);
            }

            HasCompletedIntro = persistedTopics.HasCompletedIntro;
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

            HasCompletedIntro = false;

            Persist();
        }

        public void OpenTutorial(TutorialTopic topic)
        {
            OpenTutorialRequest?.Invoke(topic);

            CompleteTopic(topic);
        }

        public void SetIntroCompleted()
        {
            HasCompletedIntro = true;
            Persist();
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
            var saveData = new TutorialSaveData(_completedChapters
                    .Where(kv => kv.Value)
                    .Select(kv => kv.Key)
                    .ToArray(),
                HasCompletedIntro);

            _persistenceService.Save(saveData);
        }
    }
}