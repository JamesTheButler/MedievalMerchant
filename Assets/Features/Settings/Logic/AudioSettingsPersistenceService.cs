using System;
using System.IO;
using Common.Infrastructure;
using Common.Infrastructure.Global;
using UnityEngine;

namespace Features.Settings.Logic
{
    public sealed class AudioSettingsPersistenceService : IService
    {
        private static readonly string FilePath = Path.Combine(PersistenceLocation.Settings, "AudioSettings");

        private ISerializer _serializer;

        private readonly AudioSettings _defaults = new(50, 50, 50, 50);

        public void Initialize()
        {
            _serializer = GlobalContext.Instance.PersistenceServices.Serializer;
        }

        public void CleanUp() { }

        public AudioSettings Load()
        {
            if (!File.Exists(FilePath))
                return _defaults;

            var fileContent = File.ReadAllText(FilePath);
            try
            {
                return _serializer.Deserialize<AudioSettings>(fileContent);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Could not deserialize {FilePath} into a {nameof(AudioSettings)} object.\n" +
                               $"Exception: {exception.Message}");
                return _defaults;
            }
        }

        public void Save(AudioSettings settings)
        {
            var directoryPath = Path.GetDirectoryName(FilePath);
            Directory.CreateDirectory(directoryPath!);

            try
            {
                var serializedData = _serializer.Serialize(settings);
                File.WriteAllText(FilePath, serializedData);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Could not serialize {settings} ({nameof(AudioSettings)}).\n" +
                               $"Exception: {exception.Message}");
            }
        }
    }
}