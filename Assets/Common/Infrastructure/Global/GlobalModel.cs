using Features.Audio.Data;

namespace Common.Infrastructure.Global
{
    public sealed class GlobalModel : IInitializable
    {
        public ProgressModel ProgressModel { get; } = new();
        public AudioSettingsModel AudioSettingsModel { get; } = new();

        public void Initialize()
        {
            ProgressModel.Initialize();
            AudioSettingsModel.Initialize();
        }

        public void CleanUp()
        {
        }
    }
}