using System.IO;
using Common.Infrastructure.Global;
using Common.Infrastructure.Serialization;

namespace Features.Audio.Data
{
    public sealed class AudioSettingsPersistenceService : FilePersistenceService<AudioSettingSaveData>
    {
        protected override string FilePath { get; } = Path.Combine(PersistenceLocation.Settings, "AudioSettings");
        protected override AudioSettingSaveData Defaults { get; } = new(50, 50, 50, 50);
    }
}