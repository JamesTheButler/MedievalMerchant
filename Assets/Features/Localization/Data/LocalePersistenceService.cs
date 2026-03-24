using System.IO;
using Common.Infrastructure.Global;
using Common.Infrastructure.Serialization;

namespace Features.Localization.Data
{
    public sealed class LocalePersistenceService : FilePersistenceService<LocaleSaveData>
    {
        protected override string FilePath { get; } = Path.Combine(PersistenceLocation.Settings, "LocaleSettings");
    }
}
