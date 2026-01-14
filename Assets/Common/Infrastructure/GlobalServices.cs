using Features.Audio;
using Features.Audio.Music;
using Features.Audio.Sfx;
using Features.Feedback.Logic;
using Features.Levels.Serialization;

namespace Common.Infrastructure
{
    public sealed class GlobalServices : IInitializable
    {
        public ISerializer Serializer { get; private set; }
        public IGamePersistenceService PersistenceService { get; private set; }
        public FeedbackService FeedbackService { get; private set; }
        public SfxService SfxService { get; private set; }
        public MusicService MusicService { get; private set; }

        public void Initialize()
        {
            Serializer = new Serializer();
            PersistenceService = new GamePersistenceService();
            FeedbackService = new FeedbackService();
            SfxService = new SfxService();
            MusicService = new MusicService();
        }

        public void CleanUp() { }
    }
}