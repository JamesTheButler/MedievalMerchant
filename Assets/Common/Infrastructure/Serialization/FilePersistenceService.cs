using System;
using System.IO;
using Common.Infrastructure.Global;
using UnityEngine;

namespace Common.Infrastructure.Serialization
{
    public abstract class FilePersistenceService<TData> : IService
    {
        protected abstract string FilePath { get; }

        private string _finalFilePath;

        protected virtual TData Defaults => default;

        private ISerializer _serializer;

        public void Initialize()
        {
            _serializer = GlobalContext.Instance.PersistenceServices.Serializer;
            _finalFilePath = Path.ChangeExtension(FilePath, "save");
        }

        public void CleanUp() { }

        public bool HasData()
        {
            return File.Exists(_finalFilePath);
        }

        public TData Load()
        {
            if (!HasData())
                return Defaults;

            var fileContent = File.ReadAllText(_finalFilePath);
            try
            {
                return _serializer.Deserialize<TData>(fileContent);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Could not deserialize {_finalFilePath} into a {nameof(TData)} object.\n" +
                               $"Exception: {exception.Message}");
                // Serialization failed, most likely due to old serialization format, so i am deleting the file.
                File.WriteAllText(_finalFilePath, string.Empty);
                return Defaults;
            }
        }

        public void Save(TData settings)
        {
            var directoryPath = Path.GetDirectoryName(_finalFilePath);
            Directory.CreateDirectory(directoryPath!);

            try
            {
                var serializedData = _serializer.Serialize(settings);
                File.WriteAllText(_finalFilePath, serializedData);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Could not serialize {settings} ({nameof(TData)}).\n" +
                               $"Exception: {exception.Message}");
            }
        }
    }
}