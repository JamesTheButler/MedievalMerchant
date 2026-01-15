using Features.Audio.Music;
using Features.Audio.Sfx;
using Features.Feedback.Logic;

namespace Common.Infrastructure.Global
{
    public sealed class GlobalServices : IInitializable
    {
        public FeedbackService FeedbackService { get; private set; }
        public SfxService SfxService { get; private set; }
        public MusicService MusicService { get; private set; }

        public void Initialize()
        {
            FeedbackService = new FeedbackService();
            SfxService = new SfxService();
            MusicService = new MusicService();
        }

        public void CleanUp() { }
    }
}