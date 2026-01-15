using System;
using System.IO;
using Common.Infrastructure.Global;
using Common.Infrastructure.Serialization;
using Features.Settings.Logic;

namespace Features.Tutorial.Logic
{
    public sealed class TutorialPersistenceService : FilePersistenceService<TutorialSaveData>
    {
        protected override string FilePath { get; } = Path.Combine(PersistenceLocation.Root, "Tutorial");
        protected override TutorialSaveData Defaults => new(Array.Empty<TutorialTopic>());
    }
}