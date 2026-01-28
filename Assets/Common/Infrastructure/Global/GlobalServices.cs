using System.Collections.Generic;
using Features.Audio.Music;
using Features.Audio.Sfx;
using Features.Feedback.Logic;
using Features.Tutorial.Logic;

namespace Common.Infrastructure.Global
{
    public sealed class GlobalServices : IInitializable
    {
        public TutorialService TutorialService { get; private set; }
        public FeedbackService FeedbackService { get; private set; }
        public SfxService SfxService { get; private set; }
        public MusicService MusicService { get; private set; }

        private readonly List<IService> _services = new();

        public void Initialize()
        {
            TutorialService = new TutorialService();
            FeedbackService = new FeedbackService();
            SfxService = new SfxService();
            MusicService = new MusicService();

            _services.Add(TutorialService);
            _services.Add(FeedbackService);
            _services.Add(SfxService);
            _services.Add(MusicService);

            foreach (var service in _services)
            {
                service.Initialize();
            }
        }

        public void CleanUp()
        {
            foreach (var service in _services)
            {
                service.CleanUp();
            }
        }
    }
}